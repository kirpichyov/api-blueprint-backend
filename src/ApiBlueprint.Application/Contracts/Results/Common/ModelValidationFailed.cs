using System.Collections.Generic;
using FluentValidation.Results;

namespace ApiBlueprint.Application.Contracts.Results.Common;

public sealed record ModelValidationFailed(IReadOnlyCollection<ValidationFailure> Failures);