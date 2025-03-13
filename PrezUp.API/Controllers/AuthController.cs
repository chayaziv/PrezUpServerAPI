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

            if (!result.IsSuccess)
            {

                return StatusCode(result.StatusCode, new{ messege =result.ErrorMessage});
            }

            return StatusCode(result.StatusCode, new { data = result.Data, messege = "User registered successfully" });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.LoginAsync(model);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });

            return StatusCode(result.StatusCode, new { data = result.Data, messege = "User logined successfully" });
        }
    }
}



