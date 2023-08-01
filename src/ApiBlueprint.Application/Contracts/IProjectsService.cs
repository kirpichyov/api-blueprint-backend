using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts.Results.Common;
using ApiBlueprint.Application.Models.Projects;
using OneOf;
using OneOf.Types;

namespace ApiBlueprint.Application.Contracts;

public interface IProjectsService
{
    Task<OneOf<ProjectSummaryResponse, ModelValidationFailed>> CreateAsync(CreateProjectRequest request);
    Task<OneOf<ProjectSummaryResponse, ResourceNotFound>> GetAsync(Guid projectId);
    Task<IReadOnlyCollection<ProjectSummaryResponse>> GetAllForUserAsync();
    Task<OneOf<Success, ResourceNotFound>> DeleteAsync(Guid projectId);

    Task<OneOf<CreatedFolderResponse, ModelValidationFailed, ResourceNotFound>> CreateFolderAsync(
        Guid projectId,
        CreateFolderRequest request);

    Task<OneOf<Success, ResourceNotFound>> DeleteFolderAsync(Guid projectId, Guid folderId);
    Task<OneOf<IReadOnlyCollection<FolderResponse>, ResourceNotFound>> GetFoldersAsync(Guid projectId);
}