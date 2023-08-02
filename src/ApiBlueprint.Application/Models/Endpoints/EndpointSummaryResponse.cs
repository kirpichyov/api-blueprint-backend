using System;
using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Application.Models.Endpoints;

public sealed record EndpointSummaryResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Path { get; init; }
    public EndpointMethod? Method { get; init; }
}