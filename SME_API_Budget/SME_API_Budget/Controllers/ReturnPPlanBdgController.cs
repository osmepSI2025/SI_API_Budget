using Microsoft.AspNetCore.Mvc;
using SME_API_Budget.Entities;
using SME_API_Budget.Services;

namespace SME_API_Budget.Controllers
{
     [Route("api/SYS-BUDGET")]
    [ApiController]
    public class ReturnPPlanBdgController : ControllerBase
    {
        private readonly IReturnPPlanBdgService _service;

        public ReturnPPlanBdgController(IReturnPPlanBdgService service)
        {
            _service = service;
        }


        [HttpGet("Return_P_Plan_Bdg/{year}/{projectCode}")]
        public async Task<IActionResult> GetAll(string year, string? projectCode = null)
        {
            var result = await _service.GetAllAsync(year, projectCode ?? "");
            var formattedData = result
             .Where(p => p.RefCode != 0) // ตรวจสอบ REF_CODE ว่ามีค่า
             .ToDictionary(
                 p => p.RefCode.ToString(), // ใช้ REF_CODE เป็น Key หลัก
                 p =>
                 {
                     var dict = new Dictionary<string, object>
                     {
                         ["DATA_P1"] = p.DataP1 ?? "",                        
                         ["REF_CODE"] = p.RefCode.ToString(),                       
                         //["YearBdg"] = p.YearBdg ?? "",
                         //["ProjectCode"] = p.ProjectCode ?? ""
                     };

                     // เพิ่มข้อมูล SubData ลงใน Dictionary
                     foreach (var sub in p.ReturnPPlanBdgSubs)
                     {

                         //              dict[sub.SubCode] = new Dictionary<string, string>
                         //              {
                         //                  ["DATA_P_S1"] = sub.DataPS1 ?? "",
                         //                  ["REF_CODE_2"] = sub.RefCode2 ?? "",
                         //                  ["BDG_TYPE"] = sub.BdgType ?? "",
                         //                  // Conditional assignment for DATA_P_S2 or DATA_P_S3
                         //                  [sub.BdgType == "เงินในงบประมาณ" ? "DATA_P_S2" : "DATA_P_S3"] = sub.BdgType == "เงินในงบประมาณ"
                         //? Convert.ToDecimal(sub.DataPS2).ToString()
                         //: Convert.ToDecimal(sub.DataPS3).ToString()
                         //              };

                         dict[sub.SubCode] = new Dictionary<string, string>
                         {
                             ["DATA_P_S1"] = sub.DataPS1 ?? "",
                             ["REF_CODE_2"] = sub.RefCode2 ?? "",
                             ["BDG_TYPE"] = sub.BdgType ?? ""
                         };

                         if (sub.BdgType == "เงินในงบประมาณ")
                         {
                             ((Dictionary<string, string>)dict[sub.SubCode])["DATA_P_S2"] = Convert.ToDecimal(sub.DataPS2).ToString();
                         }
                         else
                         {
                             ((Dictionary<string, string>)dict[sub.SubCode])["DATA_P_S3"] = Convert.ToDecimal(sub.DataPS3).ToString();
                         }
                     }

                     return dict;
                 }
             );
            return Ok(formattedData);
           // return Ok(result);
        }


        [HttpGet("Batch_Return_P_Plan_Bdg")]
     
        public async Task<IActionResult> Batch_Return_P_Plan_Bdg()
        {
            var result = await _service.BatchReturn_PlanBdg();

            return Ok("1");
            // return Ok(result);
        }
    }
}
