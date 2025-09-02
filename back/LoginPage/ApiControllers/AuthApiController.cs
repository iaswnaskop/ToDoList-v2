using System.Security.Claims;
using LoginPage.Models;
using LoginPage.Services.Login;
using Microsoft.AspNetCore.Mvc;
using LoginPage.Services.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LoginPage.ApiControllers;

    [ApiController]
    [Route("api/")]

    public class AuthApiContoller : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITasksService _tasksService;

        public AuthApiContoller(ILoginService loginService, ITasksService tasksService)
        {
            _loginService = loginService;
            _tasksService = tasksService;
        }

       
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister request)
        {
            
            var user = await _loginService.Register(request);
            
            if (user == null)
                return BadRequest("User already exists");

            return Ok("Registration Successful");
        }

       
        
        [HttpPost("login")]
        public async Task <IActionResult> Login(UserLogin request)
        {
            var result = await _loginService.Login(request);

            
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                
                if (result.ErrorMessage == "User not found")
                {
                    return NotFound(new { message = "Ο χρήστης δεν βρέθηκε." }); // 404 Not Found
                }
        
                if (result.ErrorMessage == "Wrong password")
                {
                    return Unauthorized(new { message = "Λάθος κωδικός πρόσβασης." }); // 401 Unauthorized
                }

                
                return BadRequest(new { message = result.ErrorMessage }); 
            }

            
            return Ok(new { token = result.Token });
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _loginService.GetRoles();
            
            return Ok(roles);
        }

        [Authorize]
        [HttpGet("getUserByToken")]
        public async Task<IActionResult> GetUserByToken()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("Invalid token");
                
                var user = await _loginService.GetUserById(Guid.Parse(userId));
                

                if (user == null)
                    return BadRequest("User not found");
                
                var viewModel = new UserViewModel {CurrentUser = user};
                
                viewModel.AllUsers = await _loginService.GetAllUsers();

                if (viewModel.AllUsers == null || !viewModel.AllUsers.Any())
                {
                    return Ok(viewModel);
                }

                var tasks = await _tasksService.GetTasksByUserId(user.Id);
                if (tasks == null || !tasks.Any())
                {
                    Console.WriteLine("No Task Found");
                }
                else
                {
                    var taskList = tasks;

                    foreach (var t in taskList)
                    {
                        viewModel.Tasks.Add(new Tasks
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            StatusId = t.StatusId,
                            StatusName = t.Status.Code,
                            CreatedAt = t.CreatedAt,
                            UpdatedAt = t.UpdatedAt,
                            UntilAt = t.UntilAt,
                            UserId = t.UserTasks.Select(ut => ut.UserId.ToString()).ToList()
                        });
                    }
                }


                
                
                var statusList = await _tasksService.GetStatus();
                if (statusList != null && statusList.Any())
                {
                    foreach (var s in statusList)
                    {
                        viewModel.StatusList.Add(new Status
                        {
                            Id = s.Id,
                            Name = s.Code
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No status found");
                }
                
                return Ok(viewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
           
        }

      
    }
