using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrezUp.API.PostEntity;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrezUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    [Authorize(Policy = "UserOrAdmin")]

    public class PresentationController : ControllerBase
    {
        readonly IPresentationService _presentationService;
        readonly IMapper _mapper;

        public PresentationController(IPresentationService service,IMapper mapper)
        {
            _presentationService = service;
            _mapper = mapper;
        }
        [HttpPost("analyze-audio")]
        
        public async Task<IActionResult> AnalyzeAudio([FromForm] IFormFile audio, [FromForm] bool isPublic, [FromForm] string title)
        {
            if(string.IsNullOrWhiteSpace(title))
            {
                return BadRequest(new { error = "Title is required" });
            }
            if (audio == null || audio.Length == 0)
            {
                return BadRequest(new { error = "No audio file provided" });
            }

            try
            {

             
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { error = "User ID not found in token" });
                }

                int userId = int.Parse(userIdClaim.Value);
               
           
                var result  = await _presentationService.AnalyzeAudioAsync(audio, isPublic,title, userId);
                if(result.IsSuccess)
                {
                  
                    return StatusCode(result.StatusCode, new { data = result.Data ,messege= "Audio analyzed successfully" });
                }
                else
                {
                    return StatusCode(result.StatusCode, new {messege=result.ErrorMessage});
                }
                                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _presentationService.getallAsync();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
           
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _presentationService.getByIdAsync(id);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetPublicPresentations()
        {
            var result = await _presentationService.GetPublicPresentationsAsync();
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            return StatusCode(result.StatusCode, new { data = result.Data });
        }

        [HttpDelete("{id}")]
      
        public async Task<IActionResult> DeletePresentation(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _presentationService.deleteAsync(id, userId);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, new { message = result.ErrorMessage });
            return NoContent();
        }
        
    }
}
