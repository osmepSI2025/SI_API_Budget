using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
    [Route("api/SYS-BUDGET")]
    [ApiController]
    public class RecP301Controller : ControllerBase
    {
        private readonly IRecP301Service _service;

        public RecP301Controller(IRecP301Service service)
        {
            _service = service;
        }

      

        [HttpPost("Rec_P_301")]
        public async Task<IActionResult> RecPRcSendData( [FromBody] RecP301Models Senddata)
        {
            try
            {
                var result = await _service.SendDataAsync(Senddata);

                if (result.StatusCode == 200)
                    return Ok(result);
                else
                    return StatusCode(result.StatusCode ?? 0, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }


    }
}
