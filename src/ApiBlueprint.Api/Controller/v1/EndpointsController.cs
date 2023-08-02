using System;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Extensions;
using ApiBlueprint.Application.Models.Endpoints;
using ApiBlueprint.Core.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlueprint.Api.Controller.v1;

[ApiVersion("1.0")]
public sealed class EndpointsController : ApiControllerBase
{
    private readonly IEndpointsService _endpointsService;

    public EndpointsController(IEndpointsService endpointsService)
    {
        _endpointsService = endpointsService;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEndpoint([FromRoute(Name = "id")] Guid endpointId)
    {
        var result = await _endpointsService.GetAsync(endpointId);

        return result.Match<IActionResult>(
            summary => Ok(summary),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEndpoint([FromRoute(Name = "id")] Guid endpointId)
    {
        var result = await _endpointsService.DeleteAsync(endpointId);

        return result.Match<IActionResult>(
            success => NoContent(),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }
}