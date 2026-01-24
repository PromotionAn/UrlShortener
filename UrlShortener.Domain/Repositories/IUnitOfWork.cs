using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<ShortenedUrl> ShortenedUrls { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}