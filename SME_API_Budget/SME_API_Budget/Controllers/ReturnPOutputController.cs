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

        public ReturnPOutputController(IReturnPOutputService service)
        {
            _service = service;
        }

      //  [HttpGet("Return_P_output/{year}")]
        [HttpGet("Return_P_output/{year}/{projectCode}")]
        public async Task<IActionResult> GetAll(string year, string projectCode)
        {
            var result = await _service.GetAllAsync(year, projectCode ?? "");
            return Ok(result);

          
        }


    }

}
