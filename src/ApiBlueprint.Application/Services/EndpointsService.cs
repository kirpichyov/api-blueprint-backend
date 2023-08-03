using System;
using System.Linq;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Contracts.Arguments;
using ApiBlueprint.Application.Contracts.Results.Common;
using ApiBlueprint.Application.Extensions;
using ApiBlueprint.Application.Mapping;
using ApiBlueprint.Application.Models.Endpoints;
using ApiBlueprint.Core.Constants;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.Core.Models.ValueObjects;
using ApiBlueprint.DataAccess.Contracts;
using FluentValidation;
using Kirpichyov.FriendlyJwt.Contracts;
using OneOf;
using OneOf.Types;

namespace ApiBlueprint.Application.Services;

public sealed class EndpointsService : IEndpointsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateEndpointRequest> _createValidator;
    private readonly IValidator<UpdateEndpointGeneralInfoRequest> _updateGeneralValidator;
    private readonly IValidator<UpdateEndpointContractRequest> _updateContractValidator;
    private readonly IJwtTokenReader _jwtTokenReader;
    private readonly IObjectsMapper _mapper;

    public EndpointsService(
        IUnitOfWork unitOfWork,
        IValidator<CreateEndpointRequest> createValidator,
        IValidator<UpdateEndpointGeneralInfoRequest> updateGeneralValidator,
        IValidator<UpdateEndpointContractRequest> updateContractValidator,
        IJwtTokenReader jwtTokenReader,
        IObjectsMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateGeneralValidator = updateGeneralValidator;
        _updateContractValidator = updateContractValidator;
        _jwtTokenReader = jwtTokenReader;
        _mapper = mapper;
    }

    public async Task<OneOf<EndpointResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed>> CreateAsync(
        Guid folderId,
        CreateEndpointRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ModelValidationFailed(validationResult.Errors);
        }

        var userId = _jwtTokenReader.GetUserId();
        
        var folder = await _unitOfWork.Projects.TryGetFolder(folderId, withTracking: true);
        if (folder is null || !folder.Project.HasAccess(userId))
        {
            return new ResourceNotFound("Folder");
        }

        if (!folder.Project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        var endpoint = new Endpoint(
            request.Title,
            request.Path,
            request.Method!.Value,
            folder);

        folder.AddEndpoint(endpoint);
        await _unitOfWork.CommitAsync();

        return _mapper.ToEndpointResponse(endpoint);
    }

    public async Task<OneOf<EndpointResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed>> UpdateGeneralInfoAsync(Guid endpointId, UpdateEndpointGeneralInfoRequest request)
    {
        var validationResult = await _updateGeneralValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ModelValidationFailed(validationResult.Errors);
        }
        
        var userId = _jwtTokenReader.GetUserId();
        
        var endpoint = await _unitOfWork.Endpoints.TryGet(endpointId, withTracking: true);
        if (endpoint is null || !endpoint.ProjectFolder.Project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Endpoint));
        }

        if (!endpoint.ProjectFolder.Project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        endpoint.SetTitle(request.Title);
        endpoint.SetPath(request.Path);
        endpoint.SetMethod(request.Method!.Value);
        
        await _unitOfWork.CommitAsync();

        return _mapper.ToEndpointResponse(endpoint);
    }

    public async Task<OneOf<EndpointResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed>> UpdateContractAsync(
        Guid endpointId,
        UpdateEndpointContractRequest request,
        HttpDirection httpDirection)
    {
        var validationResult = await _updateContractValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ModelValidationFailed(validationResult.Errors);
        }

        var userId = _jwtTokenReader.GetUserId();
        
        var endpoint = await _unitOfWork.Endpoints.TryGet(endpointId, withTracking: true);
        if (endpoint is null || !endpoint.ProjectFolder.Project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Endpoint));
        }

        if (!endpoint.ProjectFolder.Project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        var parameters = request.Parameters
            .Select(parameter => new EndpointParameter()
            {
                Name = parameter.Name,
                In = parameter.In!.Value,
                Notes = parameter.Notes,
                DataType = parameter.DataType,
                CreatedAtUtc = DateTime.UtcNow,
            }).ToArray();

        switch (httpDirection)
        {
            case HttpDirection.Request:
                var requestContract = endpoint.GetRequestContract();
                requestContract.SetParameters(parameters);
                requestContract.SetBody(request.ContentType, request.StatusCode, request.ContentJson);
                endpoint.SetRequestContract(requestContract);
                break;
            case HttpDirection.Response:
                var responseContract = endpoint.GetResponseContract();
                responseContract.SetParameters(parameters);
                responseContract.SetBody(request.ContentType, request.StatusCode, request.ContentJson);
                endpoint.SetResponseContract(responseContract);
                break;
            default:
                throw new ArgumentException("Value is unexpected.", nameof(httpDirection));
        }

        await _unitOfWork.CommitAsync();

        return _mapper.ToEndpointResponse(endpoint);
    }

    public async Task<OneOf<EndpointResponse, ResourceNotFound>> GetAsync(Guid endpointId)
    {
        var userId = _jwtTokenReader.GetUserId();
        
        var endpoint = await _unitOfWork.Endpoints.TryGet(endpointId, withTracking: false);
        if (endpoint is null || !endpoint.ProjectFolder.Project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Endpoint));
        }

        return _mapper.ToEndpointResponse(endpoint);
    }

    public async Task<OneOf<Success, ResourceNotFound, FlowValidationFailed>> DeleteAsync(Guid endpointId)
    {
        var userId = _jwtTokenReader.GetUserId();
        
        var endpoint = await _unitOfWork.Endpoints.TryGet(endpointId, withTracking: true);
        if (endpoint is null || !endpoint.ProjectFolder.Project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Endpoint));
        }

        if (!endpoint.ProjectFolder.Project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        _unitOfWork.Endpoints.Remove(endpoint);
        await _unitOfWork.CommitAsync();

        return default(Success);
    }
}