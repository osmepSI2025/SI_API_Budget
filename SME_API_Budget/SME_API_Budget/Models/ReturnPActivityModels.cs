using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SME_API_Budget.Models;

public partial class ReturnPActivityModels
{
    [Key]
    public string REF_CODE { get; set; } = string.Empty;

    public string? DATA_P1 { get; set; }
    public string? DATA_P2 { get; set; }
    public decimal? DATA_P3 { get; set; }
    public decimal? DATA_P4 { get; set; }
    public string? DATA_P5 { get; set; }
    public string? year_bdg { get; set; }
    public string? project_code { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; } = DateTime.Now;

    // ✅ Navigation Property
    public ICollection<ReturnPActivitySubModels> SubData { get; set; } = new List<ReturnPActivitySubModels>();
}
public partial class ReturnPActivitySubModels
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("ReturnPActivityModels")]
    public int? REF_CODE { get; set; } 

    public string SubCode { get; set; } = string.Empty;
    public string? DATA_P6 { get; set; }
    public decimal? DATA_P7 { get; set; }

    // ✅ Navigation Property
    public ReturnPActivityModels ReturnPActivity { get; set; }
}

public class ApiResponseReturnPActivityModels
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, ActivityDataModel> Data { get; set; } = new();

}
public class APIResponseDataReturnPActivityModels
{
    public Dictionary<string, ActivityDataModel> Activities { get; set; } = new();
}

public class ActivityDataModel
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
    public Dictionary<string, SubDataModel> SubData
    {
        get
        {
            return SubDataRaw
                .Where(kv => kv.Value is JsonElement)
                .ToDictionary(
                    kv => kv.Key,
                    kv => JsonSerializer.Deserialize<SubDataModel>(kv.Value.ToString() ?? "{}")
                );
        }
    }
}

public class SubDataModel
{
    public string DATA_P6 { get; set; } = string.Empty;
    public string DATA_P7 { get; set; } = "0";
}