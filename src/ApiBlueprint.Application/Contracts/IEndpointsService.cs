using System;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts.Arguments;
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
    
    Task<OneOf<EndpointResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed>> UpdateGeneralInfoAsync(
        Guid endpointId,
        UpdateEndpointGeneralInfoRequest request);

    Task<OneOf<EndpointResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed>> UpdateContractAsync(
        Guid endpointId,
        UpdateEndpointContractRequest request,
        HttpDirection httpDirection);

    Task<OneOf<EndpointResponse, ResourceNotFound>> GetAsync(Guid endpointId);
    Task<OneOf<Success, ResourceNotFound, FlowValidationFailed>> DeleteAsync(Guid endpointId);
}