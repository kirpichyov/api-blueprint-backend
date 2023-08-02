namespace ApiBlueprint.Application.Models.Projects;

public sealed record UpdateProjectRequest
{
    public string Name { get; init; }
    public string Description { get; init; }
    public bool RegenerateImage { get; init; }
}