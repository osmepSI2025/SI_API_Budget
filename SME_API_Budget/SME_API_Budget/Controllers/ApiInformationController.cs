using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
     [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ApiInformationController : ControllerBase
    {
        private readonly IApiInformationService _service;

        public ApiInformationController(IApiInformationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(MapiInformationModels apiModels)
            => Ok(await _service.GetAllServicesAsync(apiModels));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetServiceByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MapiInformation service)
        {
            await _service.AddServiceAsync(service);
            return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MapiInformation service)
        {
            if (id != service.Id) return BadRequest("ID mismatch");
            await _service.UpdateServiceAsync(service);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteServiceAsync(id);
            return NoContent();
        }
    }
}
