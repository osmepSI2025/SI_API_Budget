using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
     [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ReturnPOutcomeController : ControllerBase
    {
        private readonly IReturnPOutcomeService _service;

        public ReturnPOutcomeController(IReturnPOutcomeService service)
        {
            _service = service;
        }

        
        [HttpGet("Return_P_Outcome/{year}/{projectCode}")]
        public async Task<IActionResult> GetAll(string year, string? projectCode = null)
        {
            var result = await _service.GetAllAsync(year, projectCode ?? "");
            return Ok(result);
        }

        [HttpGet("Batch_Return_P_Outcome")]
        public async Task<IActionResult> Batch_Return_P_Outcome()
        {
            var result = await _service.BatchReturn_Outcome();
            return Ok(result);
        }
    }

}
