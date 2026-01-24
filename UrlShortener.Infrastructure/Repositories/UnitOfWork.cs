using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Repositories;
using UrlShortener.Infrastructure.Persistence;

namespace UrlShortener.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IRepository<ShortenedUrl>? _shortenedUrls;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<ShortenedUrl> ShortenedUrls
    {
        get
        {
            return _shortenedUrls ??= new Repository<ShortenedUrl>(_context);
        }
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}