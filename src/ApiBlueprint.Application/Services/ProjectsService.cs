﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Contracts.Results.Common;
using ApiBlueprint.Application.Extensions;
using ApiBlueprint.Application.Mapping;
using ApiBlueprint.Application.Models.Projects;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.Core.Options;
using ApiBlueprint.DataAccess.Contracts;
using FluentValidation;
using Kirpichyov.FriendlyJwt.Contracts;
using Microsoft.Extensions.Options;
using OneOf;
using OneOf.Types;

namespace ApiBlueprint.Application.Services;

public sealed class ProjectsService : IProjectsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateProjectRequest> _createValidator;
    private readonly ImageGenerationOptions _imageGenerationOptions;
    private readonly IJwtTokenReader _jwtTokenReader;
    private readonly IObjectsMapper _mapper;

    public ProjectsService(
        IUnitOfWork unitOfWork,
        IValidator<CreateProjectRequest> createValidator,
        IOptions<ImageGenerationOptions> imageGenerationOptions,
        IJwtTokenReader jwtTokenReader,
        IObjectsMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _jwtTokenReader = jwtTokenReader;
        _mapper = mapper;
        _imageGenerationOptions = imageGenerationOptions.Value;
    }

    public async Task<OneOf<ProjectSummaryResponse, ModelValidationFailed>> CreateAsync(CreateProjectRequest request)
    {
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ModelValidationFailed(validationResult.Errors);
        }
        
        var currentUser = await _unitOfWork.Users.TryGet(_jwtTokenReader.GetUserId(), withTracking: true);
        
        var imageUrl = _imageGenerationOptions.ProjectImageProviderUrl.Replace("{uniqueId}", Guid.NewGuid().ToString());
        var project = new Project(request.Name, request.Description, imageUrl, currentUser);

        _unitOfWork.Projects.Add(project);
        await _unitOfWork.CommitAsync();

        return _mapper.ToProjectSummaryResponse(project);
    }

    public async Task<OneOf<ProjectSummaryResponse, ResourceNotFound>> GetAsync(Guid projectId)
    {
        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: false);
        if (project is null || !project.CanEdit(_jwtTokenReader.GetUserId()))
        {
            return new ResourceNotFound(nameof(Project));
        }
        
        return _mapper.ToProjectSummaryResponse(project);
    }

    public async Task<IReadOnlyCollection<ProjectSummaryResponse>> GetAllForUserAsync()
    {
        var projects = await _unitOfWork.Projects.GetAllForUser(_jwtTokenReader.GetUserId(), withTracking: false);
        return _mapper.MapCollection(projects, _mapper.ToProjectSummaryResponse);
    }

    public async Task<OneOf<Success, ResourceNotFound>> DeleteAsync(Guid projectId)
    {
        
        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: true);
        if (project is null || !project.CanEdit(_jwtTokenReader.GetUserId()))
        {
            return new ResourceNotFound(nameof(Project));
        }
        
        _unitOfWork.Projects.Remove(project);
        await _unitOfWork.CommitAsync();

        return default(Success);
    }
}