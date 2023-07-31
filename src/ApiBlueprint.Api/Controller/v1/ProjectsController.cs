using System;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Extensions;
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

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] Guid projectId)
    {
        var result = await _projectsService.DeleteAsync(projectId);

        return result.Match<IActionResult>(
            success => NoContent(),
            notFound => NotFound(notFound.ToApiErrorResponse()));
    }
}