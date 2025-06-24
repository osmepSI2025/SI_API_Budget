using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SME_API_Budget.Models;

//public partial class RecPRModels
//{
//    public int Id { get; set; }

//    public string? YearBdg { get; set; }

//    public string? ProjectCode { get; set; }

//    public string? ActivityName { get; set; }

//    public int? RefCode { get; set; }

//    public string? DataPS1 { get; set; }

//    public string? DataPS2 { get; set; }

//    public DateTime? CreateDate { get; set; }

//    public DateTime? UpdateDate { get; set; }
//}
public class RecPRSubDataModel
{
    [JsonPropertyName("DATA_P_S1")]
    public string DATA_P_S1 { get; set; } = string.Empty;
    
    [JsonPropertyName("DATA_P_S2")]
    public string DATA_P_S2 { get; set; } = string.Empty;
}


public class ApiRecPRResponseModel
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Data { get; set; }

}

public partial class RecPRModels
{

    public string? year_bdg { get; set; }

    public string? project_code { get; set; }

    public string? Activity_name { get; set; }

    public int? ref_code { get; set; }

}
