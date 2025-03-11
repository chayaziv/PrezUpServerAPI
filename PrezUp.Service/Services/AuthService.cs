using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;
using PrezUp.Core.models;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.IdentityModel.Tokens;

namespace PrezUp.Service.Services
{

    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repository;
        private readonly IConfiguration _configuration;

        public AuthService(IRepositoryManager repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<AuthResult> RegisterUserAsync(RegisterModel model)
        {
            if (await _repository.Users.ExistsByEmailAsync(model.Email))
            {
                return new AuthResult { Succeeded = false, Errors = new List<string> { "Email already exists" } };
            }

            var newUser = new User
            {
                Email = model.Email,
                Name = model.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password) // הצפנת הסיסמה
            };

            await _repository.Users.AddAsync(newUser);
            await _repository.SaveAsync();

            var token = GenerateJwtToken(newUser);
            return new AuthResult { Succeeded = true, Token = token };
        }


        public async Task<AuthResult> LoginAsync(LoginModel model)
        {
            var user = await _repository.Users.GetByEmailAsync(model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return new AuthResult { Succeeded = false, Errors = new List<string> { "Invalid email or password" } };
            }

            var token = GenerateJwtToken(user);
            return new AuthResult { Succeeded = true, Token = token };
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


