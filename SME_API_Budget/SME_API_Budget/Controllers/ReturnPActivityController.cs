using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Models;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
    [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ReturnPActivityController : ControllerBase
    {
        private readonly IReturnPActivityService _service;

        public ReturnPActivityController(IReturnPActivityService service)
        {
            _service = service;
        }

       // [HttpGet("{year}")]
        [HttpGet("Return_P_Activity/{year}/{projectCode}")]
        public async Task<IActionResult> GetAll(string year, string? projectCode = null)
        {
            var result = await _service.GetAllAsync(year, projectCode ?? "");

            var formattedData = result
              .Where(p => p.RefCode !=0) // ตรวจสอบ REF_CODE ว่ามีค่า
              .ToDictionary(
                  p => p.RefCode.ToString(), // ใช้ REF_CODE เป็น Key หลัก
                  p =>
                  {
                      var dict = new Dictionary<string, object>
                      {
                          ["DATA_P1"] = p.DataP1 ?? "",
                          ["DATA_P2"] = p.DataP2 ?? "",
                          ["REF_CODE"] = p.RefCode.ToString(),
                          ["DATA_P3"] = p.DataP3?.ToString() ?? "0",
                          ["DATA_P4"] = p.DataP4?.ToString() ?? "0",
                          ["DATA_P5"] = p.DataP5?.ToString() ?? "0",
                          //["YearBdg"] = p.YearBdg ?? "",
                          //["ProjectCode"] = p.ProjectCode ?? ""
                      };

                      // เพิ่มข้อมูล SubData ลงใน Dictionary
                      foreach (var sub in p.ReturnPActivitySubs)
                      {
                          dict[sub.SubCode] = new Dictionary<string, string>
                          {
                              ["DATA_P6"] = sub.DataP6 ?? "",
                              ["DATA_P7"] = sub.DataP7?.ToString() ?? "0"
                          };
                      }

                      return dict;
                  }
              );
            return Ok(formattedData);
         //  return Ok(result);
        }

    
    }
}
