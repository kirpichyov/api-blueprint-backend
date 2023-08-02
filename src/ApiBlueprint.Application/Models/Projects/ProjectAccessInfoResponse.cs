using System;
using ApiBlueprint.Core.Models.Enums;

namespace ApiBlueprint.Application.Models.Projects;

public sealed record ProjectAccessInfoResponse
{
    public Guid UserId { get; init; }
    public Guid? MemberId { get; init; }
    public bool HasAccess { get; init; }
    public bool CanEdit { get; init; }
    public ProjectMemberRole? Role { get; init; }
}