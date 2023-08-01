using System;

namespace ApiBlueprint.Application.Models.Projects;

public sealed record CreatedFolderResponse
{
    public Guid Id { get; init; }
}