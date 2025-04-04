﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PrezUp.API.PostEntity;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IServices;
using PrezUp.Core.Utils;
using PrezUp.Service.Services;

namespace PrezUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  

    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        readonly IMapper _mapper;

        public UsersController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<List<UserAdminDTO>>> Get()
        {
            var result= await _userService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserAdminPost user)
        {
           
            var dto= _mapper.Map<UserAdminDTO>(user);
           
            var result = await _userService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<UserDTO>> Put(int id, [FromBody] UserAdminDTO user)
        {
            
            var result = await _userService.UpdateAdminAsync(id, user);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode);
        }
        
        
        [HttpGet("my-presentations")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<ActionResult<List<PresentationDTO>>> GetUserPresentations()
        {
          
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(new { error = "User ID not found in token" });
            }
           
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid user ID in token.");
            }

            var result = await _userService.GetPresentationsByUserIdAsync(userId);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }


    }
}
