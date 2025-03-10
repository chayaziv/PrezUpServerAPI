using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // רישום משתמש חדש
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.RegisterUserAsync(model);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "User registered successfully." });
        }

        // כניסת משתמש (Login)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var token = await _authService.LoginAsync(model);

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { Message = "Invalid username or password." });

            return Ok(new { Token = token });
        }
    }
}
}
