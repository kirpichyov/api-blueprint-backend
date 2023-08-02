using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Contracts.Results.Common;
using ApiBlueprint.Application.Extensions;
using ApiBlueprint.Application.Mapping;
using ApiBlueprint.Application.Models.Endpoints;
using ApiBlueprint.Core.Models.Entities;
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
    private readonly IJwtTokenReader _jwtTokenReader;
    private readonly IObjectsMapper _mapper;

    public EndpointsService(
        IUnitOfWork unitOfWork,
        IValidator<CreateEndpointRequest> createValidator,
        IJwtTokenReader jwtTokenReader,
        IObjectsMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
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
            return new FlowValidationFailed("Access level is low.");
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

    public async Task<OneOf<IReadOnlyCollection<EndpointSummaryResponse>, ResourceNotFound, FlowValidationFailed>> GetSummariesAsync(Guid folderId)
    {
        var userId = _jwtTokenReader.GetUserId();
        
        var folder = await _unitOfWork.Projects.TryGetFolder(folderId, withTracking: false);
        if (folder is null || !folder.Project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Endpoint));
        }

        if (!folder.Project.CanEdit(userId))
        {
            return new FlowValidationFailed("Access level is low.");
        }

        return folder.Endpoints.Select(_mapper.ToEndpointSummaryResponse).ToArray();
    }

    public async Task<OneOf<EndpointResponse, ResourceNotFound, FlowValidationFailed>> GetAsync(Guid endpointId)
    {
        var userId = _jwtTokenReader.GetUserId();
        
        var endpoint = await _unitOfWork.Endpoints.TryGet(endpointId, withTracking: false);
        if (endpoint is null || !endpoint.ProjectFolder.Project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Endpoint));
        }

        if (!endpoint.ProjectFolder.Project.CanEdit(userId))
        {
            return new FlowValidationFailed("Access level is low.");
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
            return new FlowValidationFailed("Access level is low.");
        }

        _unitOfWork.Endpoints.Remove(endpoint);
        await _unitOfWork.CommitAsync();

        return default(Success);
    }
}