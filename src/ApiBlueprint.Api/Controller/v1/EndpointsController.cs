using System;
using System.Threading.Tasks;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Contracts.Arguments;
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
            notFound => NotFound(notFound.ToApiErrorResponse()));
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

    [HttpPut("{id:guid}/general-info")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEndpoint(
        [FromRoute(Name = "id")] Guid endpointId,
        [FromBody] UpdateEndpointGeneralInfoRequest request)
    {
        var result = await _endpointsService.UpdateGeneralInfoAsync(endpointId, request);

        return result.Match<IActionResult>(
            response => Ok(response),
            modelValidationFailed => BadRequest(modelValidationFailed.ToApiErrorResponse()),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }

    [HttpPut("{id:guid}/request")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEndpointRequest(
        [FromRoute(Name = "id")] Guid endpointId,
        [FromBody] UpdateEndpointContractRequest request)
    {
        var result = await _endpointsService.UpdateContractAsync(endpointId, request, HttpDirection.Request);

        return result.Match<IActionResult>(
            response => Ok(response),
            modelValidationFailed => BadRequest(modelValidationFailed.ToApiErrorResponse()),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }
    
    [HttpPut("{id:guid}/response")]
    [ProducesResponseType(typeof(EndpointResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEndpointResponse(
        [FromRoute(Name = "id")] Guid endpointId,
        [FromBody] UpdateEndpointContractRequest request)
    {
        var result = await _endpointsService.UpdateContractAsync(endpointId, request, HttpDirection.Response);

        return result.Match<IActionResult>(
            response => Ok(response),
            modelValidationFailed => BadRequest(modelValidationFailed.ToApiErrorResponse()),
            notFound => NotFound(notFound.ToApiErrorResponse()),
            validationFailed => BadRequest(validationFailed.ToApiErrorResponse()));
    }
}