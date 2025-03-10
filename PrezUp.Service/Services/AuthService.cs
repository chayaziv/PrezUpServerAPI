using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PrezUp.Core.IRepositories;

namespace PrezUp.Service.Services
{
    class AuthService
    {
        private readonly IRepositoryManager _repository;
        private readonly IConfiguration _configuration;
       

        public AuthService(IRepositoryManager manager,
                           IConfiguration configuration)
        {
            _repository=manager;
            _configuration = configuration;
        }

        // רישום משתמש חדש
        public async Task<AuthResult> RegisterUserAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return new AuthResult { Succeeded = false, Errors = result.Errors.Select(e => e.Description).ToList() };
            }

            // אפשר להוסיף תפקידים (Roles) אם רוצים
            await _userManager.AddToRoleAsync(user, "Viewer");  // לדוגמה, לתת תפקיד "Viewer"

            return new AuthResult { Succeeded = true };
        }

        // כניסת משתמש (Login) - יצירת טוקן JWT
        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return null;

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
                return null;

            // יצירת JWT
            return GenerateJwtToken(user);
        }

        // יצירת JWT
        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // הוספת תפקידים כ-Claims
            var roles = _userManager.GetRolesAsync(user).Result;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
