﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApiBlueprint.Core.Models.Enums;
using CSharpFunctionalExtensions;

namespace ApiBlueprint.Core.Models.Entities;

public sealed class Project : EntityBase<Guid>
{
    private readonly List<ProjectMember> _projectMembers = new();
    private readonly List<ProjectFolder> _projectFolders = new();

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

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public DateTime CreatedAtUtc { get; }
    public DateTime UpdatedAtUtc { get; private set; }
    public IReadOnlyCollection<ProjectMember> ProjectMembers => _projectMembers;
    public IReadOnlyCollection<ProjectFolder> ProjectFolders => _projectFolders;

    public void SetName(string name)
    {
        Name = name;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetDescription(string description)
    {
        Description = description;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetImageUrl(string url)
    {
        ImageUrl = url;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public Result<ProjectMember> TryAddAdmin(User user)
    {
        return TryAddMember(user, ProjectMemberRole.Admin);
    }
    
    public Result<ProjectMember> TryAddViewer(User user)
    {
        return TryAddMember(user, ProjectMemberRole.Viewer);
    }

    private Result<ProjectMember> TryAddMember(User user, ProjectMemberRole role)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        if (_projectMembers.Any(member => member.UserId == user.Id))
        {
            return Result.Failure<ProjectMember>("User is already added to the project.");
        }

        var member = new ProjectMember(this, user, role);
        _projectMembers.Add(member);

        return member;
    }

    public Result TryRemoveMember(Guid memberId)
    {
        var memberToRemove = _projectMembers.FirstOrDefault(member => member.Id == memberId);
        
        if (memberToRemove is null)
        {
            return Result.Failure<ProjectMember>("Member does not exist.");
        }

        _projectMembers.Remove(memberToRemove);
        return Result.Success();
    }

    public ProjectFolder AddFolder(string name)
    {
        var folder = new ProjectFolder(name, this);
        _projectFolders.Add(folder);

        return folder;
    }

    public bool TryRemoveFolder(Guid id)
    {
        var folder = _projectFolders.FirstOrDefault(folder => folder.Id == id);
        if (folder is null)
        {
            return false;
        }
        
        _projectFolders.Remove(folder);
        return true;
    }

    public bool CanEdit(Guid userId)
    {
        return _projectMembers.Any(member => member.UserId == userId &&
                                             member.Role is ProjectMemberRole.Owner or ProjectMemberRole.Admin);
    }
    
    public bool HasAccess(Guid userId)
    {
        return _projectMembers.Any(member => member.UserId == userId);
    }
}