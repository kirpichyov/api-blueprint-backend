using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Contracts.Results.Common;
using ApiBlueprint.Application.Extensions;
using ApiBlueprint.Application.Mapping;
using ApiBlueprint.Application.Models.ProjectMembers;
using ApiBlueprint.Application.Models.Projects;
using ApiBlueprint.Core.Constants;
using ApiBlueprint.Core.Models.Entities;
using ApiBlueprint.Core.Models.Enums;
using ApiBlueprint.Core.Options;
using ApiBlueprint.DataAccess.Contracts;
using ApiBlueprint.DataAccess.Contracts.Includes;
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
    private readonly IValidator<CreateFolderRequest> _createFolderValidator;
    private readonly IValidator<UpdateFolderRequest> _updateFolderValidator;
    private readonly IValidator<AddProjectMemberRequest> _addMemberValidator;
    private readonly ImageGenerationOptions _imageGenerationOptions;
    private readonly IJwtTokenReader _jwtTokenReader;
    private readonly IObjectsMapper _mapper;

    public ProjectsService(
        IUnitOfWork unitOfWork,
        IValidator<CreateProjectRequest> createValidator,
        IValidator<CreateFolderRequest> createFolderValidator,
        IValidator<UpdateFolderRequest> updateFolderValidator,
        IValidator<AddProjectMemberRequest> addMemberValidator,
        IOptions<ImageGenerationOptions> imageGenerationOptions,
        IJwtTokenReader jwtTokenReader,
        IObjectsMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _createFolderValidator = createFolderValidator;
        _updateFolderValidator = updateFolderValidator;
        _addMemberValidator = addMemberValidator;
        _jwtTokenReader = jwtTokenReader;
        _mapper = mapper;
        _imageGenerationOptions = imageGenerationOptions.Value;
    }

    public async Task<OneOf<ProjectSummaryResponse, ModelValidationFailed>> CreateAsync(CreateProjectRequest request)
    {
        var userId = _jwtTokenReader.GetUserId();
        
        var validationResult = await _createValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ModelValidationFailed(validationResult.Errors);
        }
        
        var currentUser = await _unitOfWork.Users.TryGet(_jwtTokenReader.GetUserId(), withTracking: true);

        var imageUrl = GenerateProjectImageUrl();
        var project = new Project(request.Name, request.Description, imageUrl, currentUser);

        _unitOfWork.Projects.Add(project);
        await _unitOfWork.CommitAsync();

        return _mapper.ToProjectSummaryResponse(project, userId);
    }

    public async Task<OneOf<ProjectSummaryResponse, ResourceNotFound>> GetAsync(Guid projectId)
    {
        var userId = _jwtTokenReader.GetUserId();
        
        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: false);
        if (project is null || !project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Project));
        }
        
        return _mapper.ToProjectSummaryResponse(project, userId);
    }

    public async Task<IReadOnlyCollection<ProjectSummaryResponse>> GetAllForUserAsync()
    {
        var userId = _jwtTokenReader.GetUserId();

        var projects = await _unitOfWork.Projects.GetAllForUser(userId, withTracking: false);
        return _mapper.MapCollection(projects, project => _mapper.ToProjectSummaryResponse(project, userId));
    }

    public async Task<OneOf<ProjectSummaryResponse, ResourceNotFound, FlowValidationFailed>> UpdateAsync(Guid projectId, UpdateProjectRequest request)
    {
        var userId = _jwtTokenReader.GetUserId();

        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: true);
        if (project is null || !project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Project));
        }

        if (!project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        project.SetName(request.Name);
        project.SetDescription(request.Description);

        if (request.RegenerateImage)
        {
            var newImageUrl = GenerateProjectImageUrl();
            project.SetImageUrl(newImageUrl);
        }
        
        await _unitOfWork.CommitAsync();

        return _mapper.ToProjectSummaryResponse(project, userId);
    }

    public async Task<OneOf<Success, ResourceNotFound, FlowValidationFailed>> DeleteAsync(Guid projectId)
    {
        var userId = _jwtTokenReader.GetUserId();

        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: true);
        if (project is null || !project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Project));
        }

        if (!project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }
        
        _unitOfWork.Projects.Remove(project);
        await _unitOfWork.CommitAsync();

        return default(Success);
    }

    public async Task<ProjectAccessInfoResponse> GetProjectAccessInfoAsync(Guid projectId)
    {
        var userId = _jwtTokenReader.GetUserId();
        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: false);

        if (project is null || !project.HasAccess(userId))
        {
            return new ProjectAccessInfoResponse()
            {
                UserId = userId,
                HasAccess = false,
            };
        }

        var userAsMember = project.ProjectMembers.First(member => member.UserId == userId);

        return new ProjectAccessInfoResponse()
        {
            UserId = userId,
            MemberId = userAsMember.Id,
            HasAccess = true,
            CanEdit = project.CanEdit(userId),
            Role = userAsMember.Role,
        };
    }

    public async Task<OneOf<FolderResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed>> CreateFolderAsync(
        Guid projectId,
        CreateFolderRequest request)
    {
        var validationResult = await _createFolderValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ModelValidationFailed(validationResult.Errors);
        }

        var userId = _jwtTokenReader.GetUserId();
        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: true);

        if (project is null || !project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Project));
        }

        if (!project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        var folder = project.AddFolder(request.Name);
        await _unitOfWork.CommitAsync();

        return _mapper.ToFolderResponse(folder);
    }

    public async Task<OneOf<FolderResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed>> UpdateFolderAsync(
        Guid folderId,
        UpdateFolderRequest request)
    {
        var validationResult = await _updateFolderValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ModelValidationFailed(validationResult.Errors);
        }

        var userId = _jwtTokenReader.GetUserId();
        
        var folder = await _unitOfWork.Projects.TryGetFolder(folderId, withTracking: true);
        if (folder is null || !folder.Project.HasAccess(_jwtTokenReader.GetUserId()))
        {
            return new ResourceNotFound(nameof(Project));
        }
        
        if (!folder.Project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        folder.SetName(request.Name);
        await _unitOfWork.CommitAsync();

        return _mapper.ToFolderResponse(folder);
    }

    public async Task<OneOf<Success, ResourceNotFound, FlowValidationFailed>> DeleteFolderAsync(Guid folderId)
    {
        var userId = _jwtTokenReader.GetUserId();
        
        var folder = await _unitOfWork.Projects.TryGetFolder(folderId, withTracking: true);
        if (folder is null || !folder.Project.HasAccess(_jwtTokenReader.GetUserId()))
        {
            return new ResourceNotFound(nameof(Project));
        }
        
        if (!folder.Project.CanEdit(userId))
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        var isFolderRemoved = folder.Project.TryRemoveFolder(folderId);
        if (!isFolderRemoved)
        {
            return new ResourceNotFound("Folder");
        }
        
        await _unitOfWork.CommitAsync();

        return default(Success);
    }

    public async Task<OneOf<IReadOnlyCollection<FolderResponse>, ResourceNotFound>> GetFoldersAsync(Guid projectId)
    {
        var userId = _jwtTokenReader.GetUserId();
        
        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: false, ProjectIncludes.Endpoints);
        if (project is null || !project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Project));
        }

        return project.ProjectFolders.Select(_mapper.ToFolderResponse).ToArray();
    }

    public async Task<OneOf<IReadOnlyCollection<ProjectMemberResponse>, ResourceNotFound>> GetProjectMembersAsync(Guid projectId)
    {
        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: false, ProjectIncludes.MembersUser);
        if (project is null || !project.HasAccess(_jwtTokenReader.GetUserId()))
        {
            return new ResourceNotFound(nameof(Project));
        }

        return project.ProjectMembers.Select(_mapper.ToProjectMemberResponse).ToArray();
    }

    public async Task<OneOf<ProjectMemberResponse, ModelValidationFailed, ResourceNotFound, FlowValidationFailed, ConflictResult>> AddProjectMemberAsync(
        Guid projectId,
        AddProjectMemberRequest request)
    {
        var validationResult = await _addMemberValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return new ModelValidationFailed(validationResult.Errors);
        }
        
        var userId = _jwtTokenReader.GetUserId();

        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: true, ProjectIncludes.MembersUser);
        if (project is null || !project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Project));
        }

        var userAsMember = project.ProjectMembers.First(member => member.UserId == userId);
        
        if (userAsMember.Role is not ProjectMemberRole.Owner)
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        var userToAdd = await _unitOfWork.Users.TryGet(request.UserEmail, withTracking: true);
        if (userToAdd is null)
        {
            return new ResourceNotFound(nameof(User));
        }

        var newMemberResult = request.Role!.Value switch
        {
            ProjectMemberRoleModel.Viewer => project.TryAddViewer(userToAdd),
            ProjectMemberRoleModel.Admin => project.TryAddAdmin(userToAdd),
            _ => throw new ArgumentException("Value is unexpected.", nameof(request.Role.Value)),
        };

        if (newMemberResult.IsFailure)
        {
            return new ConflictResult(newMemberResult.Error);
        }

        await _unitOfWork.CommitAsync();

        return _mapper.ToProjectMemberResponse(newMemberResult.Value);
    }

    public async Task<OneOf<Success, ResourceNotFound, FlowValidationFailed>> RemoveProjectMemberAsync(Guid projectId, Guid memberId)
    {
        var userId = _jwtTokenReader.GetUserId();

        var project = await _unitOfWork.Projects.TryGet(projectId, withTracking: true, ProjectIncludes.MembersUser);
        if (project is null || !project.HasAccess(userId))
        {
            return new ResourceNotFound(nameof(Project));
        }

        var userAsMember = project.ProjectMembers.First(member => member.UserId == userId);
        var memberToRemove = project.ProjectMembers.FirstOrDefault(member => member.Id == memberId);
        
        if (userAsMember.Role is not ProjectMemberRole.Owner)
        {
            return new FlowValidationFailed(ErrorMessages.InsufficientRights);
        }

        if (memberToRemove is null)
        {
            return new ResourceNotFound("Project member");
        }
        
        if (memberToRemove.UserId == userId)
        {
            return new FlowValidationFailed("Self deletion is not supported.");
        }

        project.TryRemoveMember(memberId);
        await _unitOfWork.CommitAsync();

        return default(Success);
    }

    private string GenerateProjectImageUrl()
    {
        return _imageGenerationOptions.ProjectImageProviderUrl.Replace("{uniqueId}", Guid.NewGuid().ToString());
    } 
}