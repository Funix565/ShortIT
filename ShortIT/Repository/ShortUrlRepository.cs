using Microsoft.EntityFrameworkCore;
using ShortIT.Data;
using ShortIT.Interfaces;
using ShortIT.Models;

namespace ShortIT.Repository
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly ApplicationDbContext _context;

        public ShortUrlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddShortUrlAsync(ShortUrl shortUrl)
        {
            await _context.AddAsync(shortUrl);
            await _context.SaveChangesAsync();
        }

        public async Task<ShortUrl> GetShortUrlAsync(int shortUrlId)
        {
            return await _context.ShortUrls
                .Include(su => su.CreatedBy)
                .AsNoTracking()
                .FirstOrDefaultAsync(su => su.ID == shortUrlId);
        }

        public async Task<ICollection<ShortUrl>> GetShortUrlsAsync()
        {
            return await _context.ShortUrls
                .Include(su => su.CreatedBy)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ShortUrl> GetOriginalUrlAsync(string shortLink)
        {
            return await _context.ShortUrls.FirstOrDefaultAsync(su => su.ShortLink == shortLink);
        }

        public async Task RemoveShortUrlAsync(int shortUrlId)
        {
            _context.ShortUrls.Remove(
                await _context.ShortUrls.Where(su => su.ID == shortUrlId)
                .FirstOrDefaultAsync());
            await _context.SaveChangesAsync();
        }

        public async Task<bool> OriginalUrlExistsAsync(string originalUrl)
        {
            return await _context.ShortUrls.AnyAsync(su => su.OriginalUrl == originalUrl);
        }

        public async Task<bool> ShortUrlExistsAsync(int shortUrlId)
        {
            return await _context.ShortUrls.AnyAsync(su => su.ID == shortUrlId);
        }
    }
}
