using LoginPage.Models;
using LoginPage.Services.Login;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.ApiControllers;

    [ApiController]
    [Route("api/")]

    public class AuthApiContoller : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthApiContoller(ILoginService loginService)
        {
            _loginService = loginService;
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
    }
