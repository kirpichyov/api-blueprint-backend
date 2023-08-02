namespace ApiBlueprint.Application.Models.Endpoints;

public sealed record UpdateEndpointContractRequest
{
    public UpdateEndpointParameterModel[] Parameters { get; init; }
}