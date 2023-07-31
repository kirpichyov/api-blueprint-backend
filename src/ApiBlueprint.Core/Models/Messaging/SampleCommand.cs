using System;

namespace ApiBlueprint.Core.Models.Messaging;

public sealed record SampleCommand
{
    public Guid Id { get; init; }
    public int UsersCount { get; init; }
}