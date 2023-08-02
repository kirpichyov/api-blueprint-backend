using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Application.Models.Endpoints;

public sealed record UpdateEndpointParameterModel
{
    public string Name { get; init; }
    public EndpointParameterLocation? In { get; init; }
    public string DataType { get; init; }
    public string Notes { get; init; }
}