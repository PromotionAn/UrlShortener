using UrlShortener.Application.Common;
using UrlShortener.Application.DTOs;

namespace UrlShortener.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<Result<ShortenedUrlResponseDto>> CreateShortUrlAsync(string originalUrl, string baseUrl);
    Task<Result<ShortenedUrlResponseDto>> GetByShortCodeAsync(string shortCode, string baseUrl);
    Task<Result<UrlStatsResponseDto>> GetStatsAsync(string shortCode);
    Task<Result<string>> RedirectAsync(string shortCode);
}
