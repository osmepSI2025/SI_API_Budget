//using SME_API_Budget.Entities;
//using System;
//using System.Collections.Generic;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace SME_API_Budget.Models;




//public class ApiResponseReturnPExpectedModels
//{
//    public int? StatusCode { get; set; }
//    public string? Message { get; set; }
//    public Dictionary<string, ReturnPExpectedModels> Data { get; set; } = new();
//}
//public class ReturnPExpectedSubModels
//{
//    public decimal DATA_P_S1 { get; set; }
//}
//public class ReturnPExpectedModels
//{
//    public int KeyId { get; set; }
//    public string DATA_P1 { get; set; }
//    public string DATA_P2 { get; set; }

//    // Use a flexible dictionary for subdata
//    [JsonExtensionData]
//    public Dictionary<string, JsonElement> SubData { get; set; } = new();

//    // Convert JsonElement to strongly typed submodels
//    public Dictionary<string, ReturnPExpectedSubModels> GetSubData()
//    {
//        var subModels = new Dictionary<string, ReturnPExpectedSubModels>();

//        foreach (var kvp in SubData)
//        {
//            if (kvp.Value.ValueKind == JsonValueKind.Object)
//            {
//                var subModel = kvp.Value.Deserialize<ReturnPExpectedSubModels>();
//                if (subModel != null)
//                {
//                    subModels[kvp.Key] = subModel;
//                }
//            }
//        }

//        return subModels;
//    }

//}

using SME_API_Budget.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ReturnPExpectedModels
{
    public int KeyId { get; set; }
    public string DATA_P1 { get; set; }
    public string DATA_P2 { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> SubData { get; set; } = new();

    // ✅ Make sure this method is included
    public List<ReturnPExpectedSubModels> ToSubEntities()
    {
        var subEntities = new List<ReturnPExpectedSubModels>();

        foreach (var kvp in SubData)
        {
            if (kvp.Value.ValueKind == JsonValueKind.Object)
            {
                // Deserialize each sub entity
                var subModel = kvp.Value.Deserialize<ReturnPExpectedSubModels>();
                if (subModel != null)
                {
                    subModel.KeyId = KeyId;
                    subModel.SubCode = kvp.Key;
                    subEntities.Add(subModel);
                }
            }
            else if (kvp.Value.ValueKind == JsonValueKind.Number || kvp.Value.ValueKind == JsonValueKind.String)
            {
                // Handle simple number values
                if (decimal.TryParse(kvp.Value.ToString(), out decimal parsedValue))
                {
                    subEntities.Add(new ReturnPExpectedSubModels
                    {
                        KeyId = KeyId,
                        SubCode = kvp.Key,
                        DATA_P_S1 = parsedValue
                    });
                }
            }
        }

        return subEntities;
    }
}
public class ApiResponseReturnPExpectedModels
{
    public int? StatusCode { get; set; }
    public string? Message { get; set; }
    public Dictionary<string, ReturnPExpectedModels> Data { get; set; } = new();
}
public class ReturnPExpectedSubModels
{
    public int KeyId { get; set; }
    public string SubCode { get; set; }
    public decimal DATA_P_S1 { get; set; }
}
public class ReturnPExpectedApiResponse
{
    [JsonPropertyName("DATA_P1")]
    public string? DATA_P1 { get; set; }

    [JsonPropertyName("DATA_P2")]
    public string? DATA_P2 { get; set; }

    // This will capture all additional properties (like "10", "11", etc.)
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? SubData { get; set; }
}

public class ReturnPExpectedSubResponse
{
    [JsonPropertyName("DATA_P_S1")]
    public decimal DATA_P_S1 { get; set; }
}