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
        
    }
}
