using System;
using System.Text.Json.Serialization;
using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Core.Models.ValueObjects;

public sealed record EndpointParameter
{
    [JsonConstructor]
    public EndpointParameter()
    {
    }
    
    public string Name { get; init; }
    public EndpointParameterLocation In { get; init; }
    public string DataType { get; init; }
    public string Notes { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}