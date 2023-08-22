using System;
using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Application.Models.Endpoints;

public sealed record EndpointResponse
{
    public Guid Id { get; init; }
    public Guid FolderId { get; init; }
    public string Title { get; init; }
    public string Path { get; init; }
    public EndpointMethod? Method { get; init; }
    
    public EndpointDataModel Request { get; init; }
    public EndpointDataModel Response { get; init; }
}