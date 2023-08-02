namespace ApiBlueprint.Application.Models.Endpoints;

public sealed record UpdateEndpointContractRequest
{
    public UpdateEndpointParameterModel[] Parameters { get; init; }
    public string ContentType { get; init; }
    public int? StatusCode { get; init; }
    public object ContentJson { get; init; }
}