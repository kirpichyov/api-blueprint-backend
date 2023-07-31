using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBlueprint.Api.Controller;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Authorize]
public class ApiControllerBase : ControllerBase
{
}