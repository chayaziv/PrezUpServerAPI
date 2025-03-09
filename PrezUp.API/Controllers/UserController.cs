using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrezUp.Core.Entity;
using PrezUp.Core.IServices;

namespace PrezUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            return await _userService.GetAllAsync();
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
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserDTO user)
        {
            var userAdd = await _userService.AddAsync(user);
            if (userAdd != null)
                return Ok(userAdd);
            return BadRequest(userAdd);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> Put(int id, [FromBody] UserDTO user)
        {
            var userUpdate = await _userService.UpdateAsync(id, user);
            if (userUpdate != null)
                return Ok(userUpdate);
            return NotFound(userUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (!await _userService.DeleteAsync(id))
                return NotFound();
            return Ok();
        }
    }
}
