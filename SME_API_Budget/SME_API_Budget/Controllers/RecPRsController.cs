using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
    [Route("api/SYS-BUDGET")]
    [ApiController]
    public class RecPRsController : ControllerBase
    {
        private readonly IRecPRsService _service;

        public RecPRsController(IRecPRsService service)
        {
            _service = service;
        }

      

        [HttpPost("Rec_P_Rs/{year}/{projectCode}/{ref_code}")]
        public async Task<IActionResult> RecPRcSendData(string year, string projectCode, string ref_code, [FromBody] RecPRSubDataModel Senddata)
        {
            try
            {
                var result = await _service.SendDataAsync(Senddata, year, projectCode, ref_code);

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
