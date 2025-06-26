using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
    [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ReturnPAreaController : ControllerBase
    {
        private readonly IReturnPAreaService _service;

        public ReturnPAreaController(IReturnPAreaService service)
        {
            _service = service;
        }

        [HttpGet("Return_P_Area/{year}")]
        [HttpGet("Return_P_Area/{year}/{projectCode}")]
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

        [HttpGet("Return_P_Area-Batch")]
        public async Task<IActionResult> GetP_Area()
        {
            try
            {
                var projects = await _service.BatchP_Area();
                return Ok();
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
