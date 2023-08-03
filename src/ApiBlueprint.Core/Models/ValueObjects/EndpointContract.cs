using System.Text.Json.Serialization;

namespace ApiBlueprint.Core.Models.ValueObjects;

public sealed class EndpointContract
{
    public EndpointContract()
    {
    }

    [JsonConstructor]
    public EndpointContract(EndpointParameter[] parameters, object body, string contentType, int? statusCode)
    {
        Parameters = parameters;
        Body = body;
        ContentType = contentType;
        StatusCode = statusCode;
    }
    
    public EndpointParameter[] Parameters { get; private set; }
    public string ContentType { get; private set; }
    public object Body { get; private set; }
    public int? StatusCode { get; private set; }

    public void SetParameters(EndpointParameter[] parameters)
    {
        Parameters = parameters;
    }
    
    public void SetBody(string contentType, int? statusCode, object body)
    {
        ContentType = contentType;
        StatusCode = statusCode;
        Body = body;
    }
}