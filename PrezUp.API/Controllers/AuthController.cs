using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrezUp.Core.IServices;
using PrezUp.Core.models;
namespace PrezUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.RegisterUserAsync(model);

            if (!result.Succeeded)
            {
                Console.WriteLine(result.Errors.First());
                return BadRequest(new { result.Errors });
            }


            return Ok(new { result.Token, Message = "User registered successfully.", user = result.User });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.LoginAsync(model);

            if (!result.Succeeded)
                return Unauthorized(new { result.Errors });

            return Ok(new { result.Token, Message = "Login successful.", user = result.User });
        }
    }
}



