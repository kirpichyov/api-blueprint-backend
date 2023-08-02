namespace ApiBlueprint.Application.Models.Endpoints;

public sealed record EndpointDataModel
{
    public EndpointParameterModel[] Parameters { get; init; }
}