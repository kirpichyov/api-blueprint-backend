using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiBlueprint.Core.Models.ValueObjects;

public sealed class EndpointContract
{
    public EndpointContract()
    {
    }

    [JsonConstructor]
    public EndpointContract(EndpointParameter[] parameters, string bodyJson, string contentType, int? statusCode)
    {
        Parameters = parameters;
        BodyJson = bodyJson;
        ContentType = contentType;
        StatusCode = statusCode;
    }
    
    public EndpointParameter[] Parameters { get; private set; }
    public string ContentType { get; private set; }
    public string BodyJson { get; private set; }
    public int? StatusCode { get; private set; }

    public void SetParameters(EndpointParameter[] parameters)
    {
        Parameters = parameters;
    }
    
    public void SetBody(string contentType, int? statusCode, object content)
    {
        ContentType = contentType;
        StatusCode = statusCode;
        BodyJson = JsonSerializer.Serialize(content);
    }

    public object GetBodyObject() => JsonSerializer.Deserialize<object>(BodyJson);
}