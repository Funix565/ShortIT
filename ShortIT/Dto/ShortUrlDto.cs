namespace ShortIT.Dto
{
    public class ShortUrlDto
    {
        public int ID { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortLink { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUserDto CreatedBy { get; set; }
    }
}
