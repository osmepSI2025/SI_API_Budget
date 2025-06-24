using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SME_API_Budget.Models;

public partial class ReturnPPayModels
{
    public int Id { get; set; }

    public string? DATA_P1 { get; set; }

    public string? DATA_P2 { get; set; }

    public string? YearBdg { get; set; }

    public string? ProjectCode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
public class APIResponseDataReturnPPayModels
{
    [JsonPropertyName("DATA_P1")]
    public string? DATA_P1 { get; set; } // ✅ String

    [JsonPropertyName("DATA_P2")]
    public int DATA_P2 { get; set; } // ✅ Integer
}
public class ApiResponseReturnPPayModels
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public APIResponseDataReturnPPayModels Data { get; set; } = new();

}