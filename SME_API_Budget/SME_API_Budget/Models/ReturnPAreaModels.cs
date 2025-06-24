using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SME_API_Budget.Models;

public partial class ReturnPAreaModels
{
    public int Id { get; set; }

    public string? DataP1 { get; set; }

    public string? YearBdg { get; set; }

    public string? ProjectCode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
//public class ResponseReturnPArea<T>
//{
//    public int Code { get; set; }
//    public string Message { get; set; }
//    public T Data { get; set; }
//}

//public class APIResponseDataReturnPAreaModels
//{
//    [JsonPropertyName("DATA_P1")] // ✅ กำหนดให้ตรงกับ JSON
//    public string? DataP1 { get; set; }
//}
public class APIResponseDataReturnPAreaModels
{
    [JsonPropertyName("DATA_P1")] // ✅ กำหนดให้ตรงกับ JSON
    public string? DATA_P1 { get; set; }
}
public class ApiResponseReturnArea

{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public APIResponseDataReturnPAreaModels Data { get; set; } = new();
}
//public class ApiResponseReturnAreaModels

//{
//    public int? StatusCode { get; set; }
//    public string? Message { get; set; }
//    public Dictionary<string, APIResponseDataReturnPAreaModels> Data { get; set; } = new();
//}

public class ApiResponseReturnAreaModels
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public APIResponseDataReturnPAreaModels Data { get; set; } = new();
  //public string? DATA_P1 { get; set; }
    
}