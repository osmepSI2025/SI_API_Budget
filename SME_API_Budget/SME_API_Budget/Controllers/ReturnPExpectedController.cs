using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
     [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ReturnPExpectedController : ControllerBase
    {
        private readonly IReturnPExpectedService _service;

        public ReturnPExpectedController(IReturnPExpectedService service)
        {
            _service = service;
        }
        [HttpGet("Return_P_Expected/{year}")]
        [HttpGet("Return_P_Expected/{year}/{projectCode}")]
        public async Task<IActionResult> GetAll(string year, string? projectCode = null)
        {
            var result = await _service.GetAllAsync(year, projectCode ?? "");
            return Ok(result);
        }


      
    }
}
