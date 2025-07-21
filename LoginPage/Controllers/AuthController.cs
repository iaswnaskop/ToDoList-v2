using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginPage.Entities;
using LoginPage.Models;
using LoginPage.Services.Login;
using LoginPage.Services.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Status = LoginPage.Models.Status;


namespace LoginPage.Controllers
{
    
    public class LoginViewController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly ITasksService _tasksService;

        public LoginViewController(ILoginService loginService, ITasksService tasksService)
        {
            _loginService = loginService;
            _tasksService = tasksService;
        }

        public async Task<IActionResult> Register()
        {
            var roles = await _loginService.GetRoles();
            var model = new UserRegister
            {
                AvailableRoles = roles
            };
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        
        /*public async Task<IActionResult> GetTasks()
        {
            var tasks = await _tasksService.GetTasks();
            if (tasks == null)
                return NotFound("No tasks found");
            return Ok(tasks);
        }*/
        [Authorize]
        public async Task<IActionResult> LoginIndex(string token)
        {
            
            if (string.IsNullOrEmpty(token))
            {
                // Μετά τσέκαρε το Authorization header
                var authorization = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                {
                    token = authorization.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    
                    token = Request.Cookies["token"];
                }
            }
            
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");
            
            var user = await _loginService.GetUserFromToken(token);
            if (user == null)
                return RedirectToAction("Login");
            
            // Αποθήκευσε το token σε cookie αν δεν υπάρχει
            if (!Request.Cookies.ContainsKey("token"))
            {
                Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, 
                    SameSite = SameSiteMode.Strict
                });
            }

            var viewModel = new UserViewModel { CurrentUser = user };

           
            
                viewModel.AllUsers = await _loginService.GetAllUsers();
            if (viewModel.AllUsers == null || !viewModel.AllUsers.Any())
            {
                Console.WriteLine("No users found");
                return View(viewModel);
            }
            
            var tasks = await _tasksService.GetTasksByUserId (user.Id);
            if (tasks == null || !tasks.Any())
            {
                Console.WriteLine("No tasks found for user");
                return View(viewModel);
            }
               
            var tasksList = tasks;
            foreach (var t in tasksList)
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
            //var taskById = await _tasksService.GetTasksById(1);
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
                
            
            return View(viewModel); 
        }

       

    }
}
