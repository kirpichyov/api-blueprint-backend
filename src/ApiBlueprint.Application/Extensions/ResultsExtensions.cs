using System.Linq;
using ApiBlueprint.Application.Contracts.Results.Common;
using ApiBlueprint.Core.Exceptions;
using ApiBlueprint.Core.Models.Api;

namespace ApiBlueprint.Application.Extensions;

public static class ResultsExtensions
{
    public static ApiErrorResponse ToApiErrorResponse(this ModelValidationFailed result)
    {
        var nodes = result.Failures
            .Select(failure => new ApiErrorResponseNode(failure.PropertyName, failure.ErrorMessage))
            .ToArray();
        
        return new ApiErrorResponse(ExceptionsInfo.Identifiers.ModelValidationFailed, nodes);
    }

    public static ApiErrorResponse ToApiErrorResponse(this ResourceNotFound result)
    {
        var errorMessage = $"{result.ResourceName} is not found.";
        return new ApiErrorResponse(ExceptionsInfo.Identifiers.ResourceNotFound, new ApiErrorResponseNode(null, errorMessage));
    }

    public static ApiErrorResponse ToApiErrorResponse(this FlowValidationFailed result)
    {
        return new ApiErrorResponse(ExceptionsInfo.Identifiers.ValidationFailed, new ApiErrorResponseNode(null, result.Message));
    }

    public static ApiErrorResponse ToApiErrorResponse(this ConflictResult result)
    {
        return new ApiErrorResponse(ExceptionsInfo.Identifiers.ValidationFailed, new ApiErrorResponseNode(null, result.Message));
    }
}