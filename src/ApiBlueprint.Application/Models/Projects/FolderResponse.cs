using System;

namespace ApiBlueprint.Application.Models.Projects;

public sealed record FolderResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}