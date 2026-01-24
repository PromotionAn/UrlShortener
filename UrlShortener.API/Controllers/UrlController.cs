using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.API.Controllers;

public class UrlController : BaseApiController
{
    private readonly IUrlShortenerService _urlService;
    private readonly IValidator<CreateShortenedUrlDto> _validator;

    public UrlController(
        IUrlShortenerService urlService,
        IValidator<CreateShortenedUrlDto> validator)
    {
        _urlService = urlService;
        _validator = validator;
    }

    /// <summary>
    /// Creates a shortened URL
    /// </summary>
    [HttpPost("shorten")]
    [ProducesResponseType(typeof(ShortenedUrlResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ShortenUrl([FromBody] CreateShortenedUrlDto request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var result = await _urlService.CreateShortUrlAsync(request.OriginalUrl, baseUrl);

        return HandleResult(result);
    }

    /// <summary>
    /// Gets statistics for a shortened URL
    /// </summary>
    [HttpGet("stats/{shortCode}")]
    [ProducesResponseType(typeof(UrlStatsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStats(string shortCode)
    {
        var result = await _urlService.GetStatsAsync(shortCode);
        return HandleResult(result);
    }

    /// <summary>
    /// Gets details of a shortened URL
    /// </summary>
    [HttpGet("{shortCode}")]
    [ProducesResponseType(typeof(ShortenedUrlResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUrl(string shortCode)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var result = await _urlService.GetByShortCodeAsync(shortCode, baseUrl);
        return HandleResult(result);
    }
}