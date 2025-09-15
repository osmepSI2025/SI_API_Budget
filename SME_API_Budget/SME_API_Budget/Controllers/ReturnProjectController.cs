using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
    [Route("api/SYS-BUDGET/Return_Project")]
    [ApiController]
    public class ReturnProjectController : ControllerBase
    {
        private readonly IReturnProjectService _service;
        private readonly IApiInformationService _serviceApi;

        public ReturnProjectController(IReturnProjectService service, IApiInformationService serviceApi)
        {
            _service = service;         
            _serviceApi = serviceApi;
        }


        [HttpGet("Return_Project/{year}/{projectCode}")]
        public async Task<IActionResult> GetProjects(string? year, string? projectCode)
        {
            try
            {
                var projects = await _service.GetAllAsync(year, projectCode);

                return projects?.Any() == true ? Ok(projects) : NotFound(new { message = "No data" });
              
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

        [HttpGet("{year}")]
        public async Task<IActionResult> GetProjectsByYear(string? year)
        {
            try
            {
                var projects = await _service.GetAllAsync(year, null);

       
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

        [HttpGet("Batch-ReturnProject")]
        public async Task<IActionResult> Batch_ReturnProject()
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                int currentYearTh;
                if (currentYear < 2500)
                {
                    currentYearTh = currentYear + 543;
                }
                else 
                {
                    currentYearTh = currentYear;
                }
                int currentYearThto = currentYearTh+1;
                  

                // วนลูปตั้งแต่ currentYearTh ถึง currentYearTh+1
                for (int year = currentYearTh; year <= currentYearThto; year++)
                {
                    var projects = await _service.BatchAllAsync(year.ToString());
                }
                return Ok("1");

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
