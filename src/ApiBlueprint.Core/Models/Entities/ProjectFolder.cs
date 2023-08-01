using System;

namespace ApiBlueprint.Core.Models.Entities;

public sealed class ProjectFolder : EntityBase<Guid>
{
    public ProjectFolder(string name, Project project)
    {
        Name = name;
        ProjectId = project.Id;
        Project = project;
        CreatedAtUtc = DateTime.UtcNow;
    }

    private ProjectFolder()
    {
    }

    public string Name { get; }
    public Guid ProjectId { get; }
    public Project Project { get; }
    public DateTime CreatedAtUtc { get; }
}