using System;
using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Core.Models.Entities;

public sealed class ProjectMember : EntityBase<Guid>
{
    public ProjectMember(Project project, User user, ProjectMemberRole role)
    {
        Project = project;
        ProjectId = project.Id;
        User = user;
        UserId = user.Id;
        Role = role;
    }

    private ProjectMember(ProjectMemberRole role)
    {
        Role = role;
    }

    public Guid ProjectId { get; init; }
    public Project Project { get; init; }

    public Guid UserId { get; init; }
    public User User { get; init; }
    
    public ProjectMemberRole Role { get; init; }
}