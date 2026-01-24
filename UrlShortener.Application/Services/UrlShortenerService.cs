using System.Security.Cryptography;
using UrlShortener.Application.Common;
using UrlShortener.Application.DTOs;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Repositories;

namespace UrlShortener.Application.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly IUnitOfWork _unitOfWork;
    private const int ShortCodeLength = 7;

    public UrlShortenerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ShortenedUrlResponseDto>> CreateShortUrlAsync(string originalUrl, string baseUrl)
    {
        // Check if URL already exists
        var existing = await _unitOfWork.ShortenedUrls.GetAsync(u => u.OriginalUrl == originalUrl);
        
        if (existing != null)
        {
            return Result<ShortenedUrlResponseDto>.Success(MapToDto(existing, baseUrl));
        }

        // Generate unique short code
        string shortCode;
        do
        {
            shortCode = GenerateShortCode();
        } while (await _unitOfWork.ShortenedUrls.ExistsAsync(u => u.ShortCode == shortCode));

        var shortenedUrl = new ShortenedUrl(shortCode, originalUrl);
        
        await _unitOfWork.ShortenedUrls.AddAsync(shortenedUrl);
        await _unitOfWork.SaveChangesAsync();

        return Result<ShortenedUrlResponseDto>.Success(MapToDto(shortenedUrl, baseUrl));
    }

    public async Task<Result<ShortenedUrlResponseDto>> GetByShortCodeAsync(string shortCode, string baseUrl)
    {
        var url = await _unitOfWork.ShortenedUrls.GetAsync(u => u.ShortCode == shortCode);
        
        if (url == null)
        {
            return Result<ShortenedUrlResponseDto>.Failure("Short URL not found");
        }

        return Result<ShortenedUrlResponseDto>.Success(MapToDto(url, baseUrl));
    }

    public async Task<Result<UrlStatsResponseDto>> GetStatsAsync(string shortCode)
    {
        var url = await _unitOfWork.ShortenedUrls.GetAsync(u => u.ShortCode == shortCode);
        
        if (url == null)
        {
            return Result<UrlStatsResponseDto>.Failure("Short URL not found");
        }

        var stats = new UrlStatsResponseDto(
            url.ShortCode,
            url.OriginalUrl,
            url.ClickCount,
            url.CreatedAt,
            url.LastAccessedAt
        );

        return Result<UrlStatsResponseDto>.Success(stats);
    }

    public async Task<Result<string>> RedirectAsync(string shortCode)
    {
        var url = await _unitOfWork.ShortenedUrls.GetAsync(u => u.ShortCode == shortCode);
        
        if (url == null)
        {
            return Result<string>.Failure("Short URL not found");
        }

        url.IncrementClickCount();
        _unitOfWork.ShortenedUrls.Update(url);
        await _unitOfWork.SaveChangesAsync();

        return Result<string>.Success(url.OriginalUrl);
    }

    private static string GenerateShortCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var result = new char[ShortCodeLength];
        
        for (int i = 0; i < ShortCodeLength; i++)
        {
            result[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
        }
        
        return new string(result);
    }

    private static ShortenedUrlResponseDto MapToDto(ShortenedUrl entity, string baseUrl)
    {
        return new ShortenedUrlResponseDto(
            entity.ShortCode,
            $"{baseUrl}/{entity.ShortCode}",
            entity.OriginalUrl,
            entity.CreatedAt
        );
    }
}
