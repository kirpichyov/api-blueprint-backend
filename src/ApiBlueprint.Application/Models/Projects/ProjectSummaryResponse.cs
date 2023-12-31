﻿using System;
using ApiBlueprint.Application.Models.ProjectMembers;

namespace ApiBlueprint.Application.Models.Projects;

public sealed record ProjectSummaryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
    public int MembersCount { get; init; }
    public ProjectAccessInfoResponse AccessInfo { get; init; }
    public ProjectMemberResponse[] Members { get; init; }
}