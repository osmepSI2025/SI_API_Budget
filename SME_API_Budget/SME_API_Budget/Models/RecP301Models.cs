using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SME_API_Budget.Models;


public class RecPSub301Models
{
    [JsonPropertyName("DataP1")]
    public int DataP1 { get; set; } 
    
    [JsonPropertyName("DataP2")]
    public decimal DataP2 { get; set; } 
}


public class ApiRecP301ResponseModel
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Data { get; set; }

}

public partial class RecP301Models
{

    public int? yearBdg { get; set; }

    public string? projectCode { get; set; }

    public string? ActivityCame { get; set; }

    public string? refCode { get; set; }

    public List<RecPSub301Models>? Data { get; set; }
}
