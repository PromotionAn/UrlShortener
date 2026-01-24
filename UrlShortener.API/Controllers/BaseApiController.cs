using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Common;

namespace UrlShortener.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return NotFound(new { message = result.Error });
    }
}