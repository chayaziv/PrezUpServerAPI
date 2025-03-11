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
        [Authorize]
        public async Task<IActionResult> AnalyzeAudio([FromForm] IFormFile audio, [FromForm] bool isPublic)
        {
            Console.WriteLine("in AnalyzeAudio");
            if (audio == null || audio.Length == 0)
            {
                return BadRequest(new { error = "No audio file provided" });
            }

            try
            {

                // קבלת ה-UserId מתוך ה-JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { error = "User ID not found in token" });
                }

                int userId = int.Parse(userIdClaim.Value);
                Console.WriteLine("-----------------------------ללללללללללללללללללללללללללללללללללללללננננננננ: "+userId);
                // שליחת userId ל-Service
                var analysisResult = await _presentationService.AnalyzeAudioAsync(audio, isPublic, userId);
                return Ok(new { message = "Audio analyzed successfully", data = analysisResult });

                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<PresentationDTO>>> Get()
        {
            return await _presentationService.getallAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PresentationDTO>> Get(int id)
        {
            if (await _presentationService.getByIdAsync(id) == null)
                return NotFound();
            return Ok(_presentationService.getByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (!await _presentationService.deleteAsync(id))
                return NotFound();
            return Ok();
        }
       
    }
}
//[HttpPost]
//public async Task<ActionResult<PresentationDTO>> Post([FromBody] PresentationPost agreement)
//{
//    var dto = _mapper.Map<PresentationDTO>(agreement);       
//    var agreementAdd = await _presentationService.addAsync(dto);
//    if (agreementAdd != null)
//        return Ok(agreementAdd);
//    return BadRequest(agreementAdd);
//}


//[HttpPut("{id}")]
//public async Task<ActionResult<PresentationDTO>> Put(int id, [FromBody] PresentationPost agreement)
//{
//    var dto = _mapper.Map<PresentationDTO>(agreement);

//    var agreementUpdate = await _presentationService.updateAsync(id, dto);
//    if (agreementUpdate != null)
//        return Ok(agreementUpdate);
//    return NotFound(agreementUpdate);
//}