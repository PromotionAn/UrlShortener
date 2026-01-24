using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.Entities;

public class ShortenedUrl : BaseEntity
{
    public string ShortCode { get; private set; } = string.Empty;
    public string OriginalUrl { get; private set; } = string.Empty;
    public int ClickCount { get; private set; }
    public DateTime? LastAccessedAt { get; private set; }

    // EF Core requires a parameterless constructor
#pragma warning disable CS8618
    private ShortenedUrl() 
    {
    }
#pragma warning restore CS8618

    public ShortenedUrl(string shortCode, string originalUrl)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
            throw new ArgumentException("Short code cannot be empty", nameof(shortCode));
        
        if (string.IsNullOrWhiteSpace(originalUrl))
            throw new ArgumentException("Original URL cannot be empty", nameof(originalUrl));

        ShortCode = shortCode;
        OriginalUrl = originalUrl;
        ClickCount = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public void IncrementClickCount()
    {
        ClickCount++;
        LastAccessedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
