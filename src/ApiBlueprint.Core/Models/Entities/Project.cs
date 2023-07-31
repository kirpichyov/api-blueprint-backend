using System;
using System.Collections.Generic;
using System.Linq;
using ApiBlueprint.Core.Models.Enums;
using CSharpFunctionalExtensions;

namespace ApiBlueprint.Core.Models.Entities;

public sealed class Project : EntityBase<Guid>
{
    private readonly List<ProjectMember> _projectMembers = new();

    public Project(string name, string description, string imageUrl, User owner)
        : base(Guid.NewGuid())
    {
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
        TryAddMember(owner, ProjectMemberRole.Owner);
    }

    private Project()
    {
    }

    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public IReadOnlyCollection<ProjectMember> ProjectMembers => _projectMembers;

    public Result<bool> TryAddAdmin(User user)
    {
        return TryAddMember(user, ProjectMemberRole.Admin);
    }
    
    public Result<bool> TryAddViewer(User user)
    {
        return TryAddMember(user, ProjectMemberRole.Viewer);
    }

    private Result<bool> TryAddMember(User user, ProjectMemberRole role)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        if (_projectMembers.Any(member => member.UserId == user.Id))
        {
            return false;
        }
        
        _projectMembers.Add(new ProjectMember(this, user, role));
        return true;
    }

    public bool CanEdit(Guid userId)
    {
        return _projectMembers.Any(member => member.UserId == userId &&
                                             member.Role is ProjectMemberRole.Owner or ProjectMemberRole.Admin);
    }
}