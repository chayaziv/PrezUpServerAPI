using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IServices;

namespace PrezUp.API.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
      

        public TagController(ITagService tagService)
        {
            _tagService= tagService;
           
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserAdminDTO>>> Get()
        {
            var result = await _tagService.GetTagsAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, new { messege = result.ErrorMessage });
            }
            return StatusCode(result.StatusCode, new { data = result.Data });
        }
    }
}
