using Microsoft.AspNetCore.Mvc;
using PrezUp.Core.Entity;
using PrezUp.Core.IServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrezUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresentationController : ControllerBase
    {
        readonly IPresentationService _presentationService;
        //readonly IMapper _mapper;

        public PresentationController(IPresentationService service)
        {
            _presentationService = service;
            //_mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Presentation>> Get()
        {
            return _presentationService.getall();
        }

        [HttpGet("{id}")]
        public ActionResult<Presentation> Get(int id)
        {
            if (_presentationService.getById(id) == null)
                return NotFound();
            return Ok(_presentationService.getById(id));
        }

        [HttpPost]
        public async Task<ActionResult<Presentation>> Post([FromBody] AgreementPostModel agreement)
        {
            var dto = _mapper.Map<Presentation>(agreement);
            var agreementAdd = await _presentationService.addAsync(dto);
            if (agreementAdd != null)
                return Ok(agreementAdd);
            return BadRequest(agreementAdd);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Presentation>> Put(int id, [FromBody] Presentation agreement)
        {
            //var dto = _mapper.Map<Presentation>(agreement);
           var dto = agreement;
            var agreementUpdate = await _presentationService.updateAsync(id, dto);
            if (agreementUpdate != null)
                return Ok(agreementUpdate);
            return NotFound(agreementUpdate);

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
