using SME_API_Budget.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SME_API_Budget.Models;

public partial class ReturnPPlanBdgModels
{
    public int Id { get; set; }

    public string? DataP1 { get; set; }

    public string? DataPS1 { get; set; }

    public int? RefCode { get; set; }

    public string? YearBdg { get; set; }

    public string? ProjectCode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    // ✅ Navigation Property
    public ICollection<ReturnPPlanBdgSubModels> SubData { get; set; } = new List<ReturnPPlanBdgSubModels>();
}

public partial class ReturnPPlanBdgSubModels
{
    public int Id { get; set; }

    public string SubCode { get; set; } = null!;

    public int RefCode { get; set; }

    public string DataPS1 { get; set; } = null!;

    public decimal DataPS2 { get; set; }

    public string RefCode2 { get; set; } = null!;

    public string BdgType { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

  
}

public class ApiResponseReturnPPlanBdgModels
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, PlanBdgDataModel> Data { get; set; } = new();

}
public class PlanBdgDataModel
{
    public string DATA_P1 { get; set; } = string.Empty;
    public string DATA_P2 { get; set; } = string.Empty;
    public string REF_CODE { get; set; } = string.Empty;
    public string DATA_P3 { get; set; } = "0";
    public string DATA_P4 { get; set; } = "0";
    public string DATA_P5 { get; set; } = string.Empty;

    [JsonExtensionData]
    public Dictionary<string, object> SubDataRaw { get; set; } = new();

    [JsonIgnore]
    public Dictionary<string, PlanSubDataModel> SubData
    {
        get
        {
            return SubDataRaw
                .Where(kv => kv.Value is JsonElement)
                .ToDictionary(
                    kv => kv.Key,
                    kv => JsonSerializer.Deserialize<PlanSubDataModel>(kv.Value.ToString() ?? "{}")
                );
        }
    }
}
public class PlanSubDataModel
{
    public string DATA_P_S1 { get; set; } = string.Empty;
    public string DATA_P_S2 { get; set; } = string.Empty;
    public string REF_CODE_2 { get; set; } = string.Empty;
    public string BDG_TYPE { get; set; } = string.Empty;
}