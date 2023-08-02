using System;
using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Application.Models.ProjectMembers;

public sealed record ProjectMemberResponse
{
    public Guid MemberId { get; init; }
    public ProjectMemberRole Role { get; init; }
    public UserModel User { get; init; }
}