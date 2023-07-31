namespace ApiBlueprint.Application.Models.Projects;

public sealed record CreateProjectRequest
{
    public string Name { get; init; }
    public string Description { get; init; }
}