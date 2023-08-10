using System;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Extensions;
using ApiBlueprint.Application.Models.Endpoints;
using ApiBlueprint.Application.Models.Projects;
using ApiBlueprint.Core.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlueprint.Api.Controller.v1;

[ApiVersion("1.0")]
public sealed class FoldersController : ApiControllerBase
{
    private readonly IEndpointsService _endpointsService;
    private readonly IProjectsService _projectsService;

    public FoldersController(
        IEndpointsService endpointsService,
        IProjectsService projectsService)
    {
        _endpointsService = endpointsService;
        _projectsService = projectsService;
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFolder([FromRoute(Name = "id")] Guid folderId)
    {
        var result = await _projectsService.DeleteFolderAsync(folderId);

        return result.Match<IActionResult>(
            response => NoContent(),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(FolderResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFolder([FromRoute(Name = "id")] Guid folderId, [FromBody] UpdateFolderRequest request)
    {
        var result = await _projectsService.UpdateFolderAsync(folderId, request);

        return result.Match<IActionResult>(
            response => Ok(response),
            modelValidationFailed =>  BadRequest(modelValidationFailed.ToApiErrorResponse()),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }

    [HttpPost("{id:guid}/endpoints")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateEndpoint(
        [FromRoute(Name = "id")] Guid folderId,
        [FromBody] CreateEndpointRequest request)
    {
        var result = await _endpointsService.CreateAsync(folderId, request);

        return result.Match<IActionResult>(
            summary => StatusCode(StatusCodes.Status201Created, summary),
            modelValidationFailed => BadRequest(modelValidationFailed.ToApiErrorResponse()),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }
}