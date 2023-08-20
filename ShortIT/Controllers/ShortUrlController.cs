using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShortIT.Dto;
using ShortIT.Interfaces;
using ShortIT.Models;

namespace ShortIT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlController : ControllerBase
    {
        private readonly IShortUrlRepository _shortUrlRepository;
        private readonly IMapper _mapper;

        public ShortUrlController(IShortUrlRepository shortUrlRepository, IMapper mapper)
        {
            _shortUrlRepository = shortUrlRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetShortUrlsAsync()
        {
            var shortUrls = _mapper.Map<List<ShortUrlDto>>(await _shortUrlRepository.GetShortUrlsAsync());
            return Ok(shortUrls);
        }

        [HttpGet("{shortUrlId:int}")]
        public async Task<IActionResult> GetShortUrlsAsync(int shortUrlId)
        {
            var shortUrl = _mapper.Map<ShortUrlDto>(await _shortUrlRepository.GetShortUrlAsync(shortUrlId));

            if (shortUrl == null)
            {
                return NotFound();
            }

            return Ok(shortUrl);
        }

        [HttpGet("{shortLink}")]
        public async Task<IActionResult> GetOriginalUrlAsync(string shortLink)
        {
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/api/shorturl/";

            var originalUrl = await _shortUrlRepository.GetOriginalUrlAsync(baseUrl + shortLink);

            if (originalUrl == null)
            {
                return NotFound();
            }

            return Redirect(originalUrl.OriginalUrl);
        }

        [HttpDelete("{shortUrlId}")]
        public async Task<IActionResult> DeleteShortUrlAsync(int shortUrlId)
        {
            if (! await _shortUrlRepository.ShortUrlExistsAsync(shortUrlId))
            {
                return NotFound();
            }

            await _shortUrlRepository.RemoveShortUrlAsync(shortUrlId);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortUrlAsync([FromForm] string originalUrl)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(originalUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            // Validate URL for API backend.
            if (!result)
            {
                return BadRequest(originalUrl);
            }

            // TODO: How to process CreatedBy?

            if (await _shortUrlRepository.OriginalUrlExistsAsync(originalUrl))
            {
                return Conflict(originalUrl);
            }

            // TODO: I am not sure. Should we use backend url or url from Angular?
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/api/shorturl/";
            var shortResult = baseUrl + ShortenUrl(originalUrl);

            ShortUrl toAdd = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortLink = shortResult,
                CreatedDate = DateTime.UtcNow
            };

            await _shortUrlRepository.AddShortUrlAsync(toAdd);

            return CreatedAtAction(nameof(CreateShortUrlAsync), new { ID = toAdd.ID }, toAdd);
        }

        // TODO: I don't like that it can return only numbers. How to imptove that? Will some encoding help?
        private string ShortenUrl(string originalUrl)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(originalUrl);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).Substring(0, 7);
            }
        }
    }
}
