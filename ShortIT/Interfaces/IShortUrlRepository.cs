using ShortIT.Models;

namespace ShortIT.Interfaces
{
    public interface IShortUrlRepository
    {
        Task<ICollection<ShortUrl>> GetShortUrlsAsync();

        Task<ShortUrl> GetShortUrlAsync(int shortUrlId);

        Task<ShortUrl> GetOriginalUrlAsync(string shortLink);

        Task AddShortUrlAsync(ShortUrl shortUrl);

        Task RemoveShortUrlAsync(int shortUrlId);

        Task<bool> OriginalUrlExistsAsync(string originalUrl);

        Task<bool> ShortUrlExistsAsync(int shortUrlId);
    }
}
