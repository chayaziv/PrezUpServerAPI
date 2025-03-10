using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrezUp.API.PostEntity;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IServices;
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
        [Authorize]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            var users= await _userService.GetAllAsync();
            if (users == null)
            {
                return NotFound("no users found");
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserPost user)
        {
            var dto= _mapper.Map<UserDTO>(user);
            var userAdd = await _userService.AddAsync(dto);
            if (userAdd != null)
                return Ok(userAdd);
            return BadRequest(userAdd);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> Put(int id, [FromBody] UserPost user)
        {
            var dto = _mapper.Map<UserDTO>(user);
            var userUpdate = await _userService.UpdateAsync(id, dto);
            if (userUpdate != null)
                return Ok(userUpdate);
            return NotFound(userUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await _userService.DeleteAsync(id))
                return NotFound();
            return Ok();
        }
        
        [HttpGet("{userId}/presentations")]
        public async Task<ActionResult<List<PresentationDTO>>> GetUserPresentations(int userId)
        {
            var presentations = await _userService.GetPresentationsByUserIdAsync(userId);

            if (presentations == null || presentations.Count == 0)
            {
                return NotFound("No presentations found for this user.");
            }

            return Ok(presentations);
        }

    }
}
