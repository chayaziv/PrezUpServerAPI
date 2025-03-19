using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IServices;

namespace PrezUp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<AdminDashboardDto>> GetDashboardData()
        {
            var result = await _adminService.GetDashboardDataAsync();
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
            return Ok(new {data= result.Data});
        }
    }
}
