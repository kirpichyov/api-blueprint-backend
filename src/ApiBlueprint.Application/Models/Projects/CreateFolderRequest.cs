namespace ApiBlueprint.Application.Models.Projects;

public sealed record CreateFolderRequest
{
    public string Name { get; init; }
}