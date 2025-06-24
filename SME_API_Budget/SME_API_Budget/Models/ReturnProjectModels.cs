using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SME_API_Budget.Models;

public partial class ReturnProjectModels
{
    //public int Id { get; set; }
    [JsonPropertyName("DATA_P1")]
    public string? DATA_P1 { get; set; }
    [JsonPropertyName("DATA_P2")]
    public string? DATA_P2 { get; set; }
    [JsonPropertyName("DATA_P3")]
    public string? DATA_P3 { get; set; }
    [JsonPropertyName("DATA_P4")]
    public DateTime? DATA_P4 { get; set; }
    [JsonPropertyName("DATA_P5")]
    public DateTime? DATA_P5 { get; set; }
    [JsonPropertyName("DATA_P6")]
    public string? DATA_P6 { get; set; }
    [JsonPropertyName("DATA_P7")]
    public string? DATA_P7 { get; set; }
    [JsonPropertyName("DATA_P8")]
    public string? DATA_P8 { get; set; }
    [JsonPropertyName("DATA_P9")]
    public string? DATA_P9 { get; set; }
    [JsonPropertyName("DATA_P10")]
    public string? DATA_P10 { get; set; }
    [JsonPropertyName("DATA_P11")]
    public string? DATA_P11 { get; set; }
    [JsonPropertyName("DATA_P12")]
    public decimal? DATA_P12 { get; set; }
    [JsonPropertyName("DATA_P13")]
    public decimal? DATA_P13 { get; set; }
    
    //public string? YearBdg { get; set; }

    //public string? ProjectCode { get; set; }

    //public DateTime? CreateDate { get; set; }

    //public DateTime? UpdateDate { get; set; }
  //  public int? KeyId { get; set; }
}
//public class ResponseReturnProject<T>
//{
//    public int Code { get; set; }
//    public string Message { get; set; }
//    public T Data { get; set; }
//}
public class APIResponseReturnProjectModels
{

    [JsonPropertyName("DATA_P1")]
    public string? DATA_P1 { get; set; }
    [JsonPropertyName("DATA_P2")]
    public string? DATA_P2 { get; set; }
    [JsonPropertyName("DATA_P3")]
    public string? DATA_P3 { get; set; }
    [JsonPropertyName("DATA_P4")]
    public DateTime? DATA_P4 { get; set; }
    [JsonPropertyName("DATA_P5")]
    public DateTime? DATA_P5 { get; set; }
    [JsonPropertyName("DATA_P6")]
    public string? DATA_P6 { get; set; }
    [JsonPropertyName("DATA_P7")]
    public string? DATA_P7 { get; set; }
    [JsonPropertyName("DATA_P8")]
    public string? DATA_P8 { get; set; }
    [JsonPropertyName("DATA_P9")]
    public string? DATA_P9 { get; set; }
    [JsonPropertyName("DATA_P10")]
    public string? DATA_P10 { get; set; }
    [JsonPropertyName("DATA_P11")]
    public string? DATA_P11 { get; set; }
    [JsonPropertyName("DATA_P12")]
    public decimal? DATA_P12 { get; set; }
    [JsonPropertyName("DATA_P13")]
    public decimal? DATA_P13 { get; set; }
}
public class ApiResponseReturnProjectModels
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, APIResponseReturnProjectModels> Data { get; set; } = new();
}
