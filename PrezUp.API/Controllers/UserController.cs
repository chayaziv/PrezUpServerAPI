using System.Security.Claims;
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
    [Authorize(Policy = "AdminOnly")]

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
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            var result= await _userService.GetAllAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpGet("{id}")]
        
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
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserPost user)
        {
           
            var dto= _mapper.Map<UserDTO>(user);
           
            var result = await _userService.AddAsync(dto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> Put(int id, [FromBody] UserPost user)
        {
            var dto = _mapper.Map<UserDTO>(user);
            var result = await _userService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpDelete("{id}")]
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
