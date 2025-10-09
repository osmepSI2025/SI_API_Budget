using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
     [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ReturnPOutputController : ControllerBase
    {
        private readonly IReturnPOutputService _service;
        private readonly ILogger<ReturnPOutputController> _logger;
        public ReturnPOutputController(IReturnPOutputService service
            , ILogger<ReturnPOutputController> logger
            )
        {
            _service = service;
            _logger = logger;

        }

      
        [HttpGet("Return_P_output/{year}/{projectCode}")]
        public async Task<IActionResult> GetAll(string year, string projectCode)
        {
            _logger.LogInformation($"start Return_P_output");
            var result = await _service.GetAllAsync(year, projectCode);
            return Ok(result);

          
        }
        [HttpGet("Batch_Return_P_output")]
        public async Task<IActionResult> Batch_Return_P_output()
        {
            var result = await _service.Batch_Return_Output();
            return Ok(result);


        }

    }

}
