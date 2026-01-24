using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.API.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly IUrlShortenerService _urlService;

    public RedirectController(IUrlShortenerService urlService)
    {
        _urlService = urlService;
    }

    /// <summary>
    /// Redirects to the original URL
    /// </summary>
    [HttpGet("/r/{shortCode}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RedirectToUrl(string shortCode)
    {
        var result = await _urlService.RedirectAsync(shortCode);

        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Error });
        }

        return Redirect(result.Data!);
    }
}
