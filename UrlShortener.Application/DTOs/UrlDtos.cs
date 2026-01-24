namespace UrlShortener.Application.DTOs;

public record CreateShortenedUrlDto(string OriginalUrl);

public record ShortenedUrlResponseDto(
    string ShortCode,
    string ShortUrl,
    string OriginalUrl,
    DateTime CreatedAt
);

public record UrlStatsResponseDto(
     string ShortCode,
    string OriginalUrl,
    int ClickCount,
    DateTime CreatedAt,
    DateTime? LastAccessedA
);