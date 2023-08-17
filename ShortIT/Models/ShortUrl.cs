using Microsoft.EntityFrameworkCore;

namespace ShortIT.Models
{
    [Index(nameof(OriginalUrl), IsUnique = true)]
    public class ShortUrl
    {
        public int ID { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortLink { get; set; }
        public DateTime CreatedDate { get; set; }

        public ApplicationUser CreatedBy { get; set; }
    }
}
