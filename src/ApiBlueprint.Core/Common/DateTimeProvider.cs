using System;
using ApiBlueprint.Core.Contracts;

namespace ApiBlueprint.Core.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}