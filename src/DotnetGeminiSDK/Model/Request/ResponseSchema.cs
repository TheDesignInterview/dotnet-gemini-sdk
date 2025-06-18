using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotnetGeminiSDK.Model.Request;

/// <summary>
/// Represents the overall schema for the response.
/// Based on a subset of the OpenAPI 3.0 schema.
/// </summary>
public class ResponseSchema
{
    [JsonProperty("type")] public string Type { get; set; }

    [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, ResponseSchema> Properties { get; set; }

    [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
    public ResponseSchema Items { get; set; }

    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }
}