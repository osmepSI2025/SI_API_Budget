using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SME_API_Budget.Models;

public partial class ReturnPRiskModels
{
   // public int Id { get; set; }

  
    [JsonPropertyName("DATA_P1")]
    public string? DATA_P1 { get; set; }
    [JsonPropertyName("DATA_P2")]
    public string? DATA_P2 { get; set; }
    [JsonPropertyName("DATA_P3")]
    public string? DATA_P3 { get; set; }
    [JsonPropertyName("DATA_P4")]
    public string? DATA_P4 { get; set; }
    [JsonPropertyName("DATA_P5")]
    public string? DATA_P5 { get; set; }

    [JsonPropertyName("REF_CODE")]
    public int? RefCode { get; set; }

    //public string? YearBdg { get; set; }

    //public string? ProjectCode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
    public int? KeyId { get; set; }
}

public class APIResponseDataReturnPRiskModels
{

    [JsonPropertyName("DATA_P1")]
    public string? DATA_P1 { get; set; }
    [JsonPropertyName("DATA_P2")]
    public string? DATA_P2 { get; set; }
    [JsonPropertyName("DATA_P3")]
    public string? DATA_P3 { get; set; }
    [JsonPropertyName("DATA_P4")]
    public string? DATA_P4 { get; set; }
    [JsonPropertyName("DATA_P5")]
    public string? DATA_P5 { get; set; }
    [JsonPropertyName("REF_CODE")]
    public int? RefCode { get; set; }
}
public class ApiResponseReturnPRiskModels
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, APIResponseDataReturnPRiskModels> Data { get; set; } = new();
}