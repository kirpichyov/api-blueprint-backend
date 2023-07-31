using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiBlueprint.Application.Contracts;
using ApiBlueprint.Application.Models.Profile;

namespace ApiBlueprint.Api.Controller.v1;

[ApiVersion("1.0")]
public sealed class ProfileController : ApiControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("current")]
    [ProducesResponseType(typeof(CurrentUserProfileResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentProfile()
    {
        var response = await _profileService.GetCurrentProfile();
        return Ok(response);
    }
}