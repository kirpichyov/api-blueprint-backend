using System;
using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Application.Models.Projects;

public sealed record FolderEndpointModel
{
    public Guid Id { get; init; }
    public Guid FolderId { get; init; }
    public string Title { get; init; }
    public string Path { get; init; }
    public EndpointMethod? Method { get; init; }
}