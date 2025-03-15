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
using AutoMapper;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.Utils;

namespace PrezUp.Service.Services
{

    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IValidatorService _validator;

        public AuthService(IRepositoryManager repository, IConfiguration configuration, IMapper mapper, IValidatorService validator)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _validator = validator;
        }
 
        public async Task<Result<AuthData>> RegisterUserAsync(RegisterModel model)
        {




            var userDto = new UserDTO
            {
                Email = model.Email,
                Name = model.UserName,
                Password = model.Password
            };

          
            var validorUser = await _validator.ValidateUserAsync(userDto);
            if(!validorUser.IsValid)
            {
                return Result<AuthData>.BadRequest(validorUser.Message);
            }
            var newUser = _mapper.Map<User>(userDto);

            await _repository.Users.AddAsync(newUser);
            await _repository.SaveAsync();

            userDto = _mapper.Map<UserDTO>(newUser);
            var token = GenerateJwtToken(newUser);
   
            return Result<AuthData>.Success(new AuthData() { Token = token, User = userDto });
        }


        public async Task<Result<AuthData>> LoginAsync(LoginModel model)
        {
            var existUser = await _repository.Users.GetByEmailAsync(model.Email);
            if (existUser == null)
            {
              
                return Result<AuthData>.BadRequest("Invalid email");
            }
            if (!BCrypt.Net.BCrypt.Verify(model.Password, existUser.PasswordHash))
            {

                return Result<AuthData>.BadRequest("Invalid password");
            }
            var userDto = _mapper.Map<UserDTO>(existUser);
            var token = GenerateJwtToken(existUser);
         
            return Result<AuthData>.Success(new AuthData() { Token = token, User = userDto });
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


