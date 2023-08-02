﻿using System;

namespace ApiBlueprint.Application.Models.Projects;

public sealed record FolderSummaryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}