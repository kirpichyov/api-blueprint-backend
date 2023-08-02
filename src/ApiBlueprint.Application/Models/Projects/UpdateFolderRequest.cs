namespace ApiBlueprint.Application.Models.Projects;

public sealed record UpdateFolderRequest
{
    public string Name { get; init; }
}