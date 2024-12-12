using System.ComponentModel;

namespace URLShortenerAngular.Server.Models
{
    public class UrlItem
    {
        public int Id { get; set; }

        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        [DefaultValue(0)]
        public int TransitionsCount { get; set; }
        public DateTime CreatedDate { get; set; }

        public int AuthorId { get; set; }

        public virtual User Author { get; set; }
    }
}
