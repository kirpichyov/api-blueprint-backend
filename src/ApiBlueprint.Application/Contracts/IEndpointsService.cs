using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts.Results.Common;
using ApiBlueprint.Application.Models.Endpoints;
using OneOf;
using OneOf.Types;

namespace ApiBlueprint.Application.Contracts;

public interface IEndpointsService
{
    Task<OneOf<EndpointResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed>> CreateAsync(
        Guid folderId,
        CreateEndpointRequest request);

    Task<OneOf<IReadOnlyCollection<EndpointSummaryResponse>, ResourceNotFound, FlowValidationFailed>> GetSummariesAsync(Guid folderId);
    Task<OneOf<EndpointResponse, ResourceNotFound, FlowValidationFailed>> GetAsync(Guid endpointId);
    Task<OneOf<Success, ResourceNotFound, FlowValidationFailed>> DeleteAsync(Guid endpointId);
}