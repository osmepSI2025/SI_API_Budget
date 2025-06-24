using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
     [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ReturnPRiskController : ControllerBase
    {
        private readonly IReturnPRiskService _service;

        public ReturnPRiskController(IReturnPRiskService service)
        {
            _service = service;
        }

        [HttpGet("Return_P_Risk/{year}")]
        [HttpGet("Return_P_Risk/{year}/{projectCode}")]
        public async Task<IActionResult> GetAll(string year, string? projectCode = null)
        {
            var result = await _service.GetAllAsync(year, projectCode ?? "");
            return Ok(result);
        }

    }
}
