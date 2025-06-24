using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
     [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ReturnPPayController : ControllerBase
    {
        private readonly IReturnPPayService _service;

        public ReturnPPayController(IReturnPPayService service)
        {
            _service = service;
        }

        [HttpGet("Return_P_Pay/{year}")]
        [HttpGet("Return_P_Pay/{year}/{projectCode}")]
        public async Task<IActionResult> GetAll(string year, string? projectCode = null)
        {
            try
            {
                var projects = await _service.GetAllAsync(year, projectCode);


                return Ok(projects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }


        
    }
}
