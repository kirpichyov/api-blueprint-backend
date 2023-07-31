using System;

namespace ApiBlueprint.Core.Contracts;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}