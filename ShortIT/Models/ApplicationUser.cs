using Microsoft.AspNetCore.Identity;

namespace ShortIT.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ShortUrl> ShortUrls { get; set; }
    }
}