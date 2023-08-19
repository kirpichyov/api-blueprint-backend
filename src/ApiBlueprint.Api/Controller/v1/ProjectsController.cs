using System;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Extensions;
using ApiBlueprint.Application.Models.ProjectMembers;
using ApiBlueprint.Application.Models.Projects;
using ApiBlueprint.Core.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlueprint.Api.Controller.v1;

[ApiVersion("1.0")]
public sealed class ProjectsController : ApiControllerBase
{
    private readonly IProjectsService _projectsService;

    public ProjectsController(IProjectsService projectsService)
    {
        _projectsService = projectsService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProjectSummaryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
    {
        var result = await _projectsService.CreateAsync(request);

        return result.Match<IActionResult>(
            summary => StatusCode(StatusCodes.Status201Created, summary),
            modelValidationFailed => BadRequest(modelValidationFailed.ToApiErrorResponse()));
    }
    
    [HttpGet("summary")]
    [ProducesResponseType(typeof(ProjectSummaryResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var result = await _projectsService.GetAllForUserAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProjectSummaryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute(Name = "id")] Guid projectId)
    {
        var result = await _projectsService.GetAsync(projectId);

        return result.Match<IActionResult>(
            summary => Ok(summary),
            notFound => NotFound(notFound.ToApiErrorResponse()));
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        [FromRoute(Name = "id")] Guid projectId,
        [FromBody] UpdateProjectRequest request)
    {
        var result = await _projectsService.UpdateAsync(projectId, request);

        return result.Match<IActionResult>(
            response => Ok(response),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()),
            flowValidationFailed => BadRequest(flowValidationFailed.ToApiErrorResponse()));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] Guid projectId)
    {
        var result = await _projectsService.DeleteAsync(projectId);

        return result.Match<IActionResult>(
            success => NoContent(),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }

    [HttpPost("{id:guid}/folders")]
    [ProducesResponseType(typeof(FolderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateFolder(
        [FromRoute(Name = "id")] Guid projectId,
        [FromBody] CreateFolderRequest request)
    {
        var result = await _projectsService.CreateFolderAsync(projectId, request);

        return result.Match<IActionResult>(
            response => StatusCode(StatusCodes.Status201Created, response),
            modelValidationFailed => BadRequest(modelValidationFailed.ToApiErrorResponse()),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }
    
    [HttpGet("{id:guid}/folders")]
    [ProducesResponseType(typeof(FolderResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFolders([FromRoute(Name = "id")] Guid projectId)
    {
        var result = await _projectsService.GetFoldersAsync(projectId);

        return result.Match<IActionResult>(
            response => Ok(response),
            notFound => NotFound(notFound.ToApiErrorResponse()));
    }

    [HttpGet("{id:guid}/access-info")]
    [ProducesResponseType(typeof(ProjectAccessInfoResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccessInfo([FromRoute(Name = "id")] Guid projectId)
    {
        var result = await _projectsService.GetProjectAccessInfoAsync(projectId);
        return Ok(result);
    }

    [HttpGet("{id:guid}/members")]
    [ProducesResponseType(typeof(ProjectMemberResponse[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProjectMembers([FromRoute(Name = "id")] Guid projectId)
    {
        var result = await _projectsService.GetProjectMembersAsync(projectId);

        return result.Match<IActionResult>(
            response => Ok(response),
            notFound => NotFound(notFound.ToApiErrorResponse()));
    }

    [HttpPost("{id:guid}/members")]
    [ProducesResponseType(typeof(ProjectMemberResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddProjectMember(
        [FromRoute(Name = "id")] Guid projectId,
        [FromBody] AddProjectMemberRequest request)
    {
        var result = await _projectsService.AddProjectMemberAsync(projectId, request);

        return result.Match<IActionResult>(
            response => StatusCode(StatusCodes.Status201Created, response),
            modelValidationFailed => BadRequest(modelValidationFailed.ToApiErrorResponse()),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()),
            conflict => Conflict(conflict.ToApiErrorResponse()));
    }

    [HttpDelete("{id:guid}/members/{memberId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveProjectMember(
        [FromRoute(Name = "id")] Guid projectId,
        [FromRoute] Guid memberId)
    {
        var result = await _projectsService.RemoveProjectMemberAsync(projectId, memberId);

        return result.Match<IActionResult>(
            response => NoContent(),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }
}