using System.ComponentModel.DataAnnotations;

namespace Bethel_Heritage_Academy_WebSite.Services
{
    public class NewsTableVM
    {
        public int? Id { get; set; }
        [StringLength(150)]
        public string Reporter { get; set; }
        [StringLength(250)]
        public string Headline { get; set; }
        public string NewBody { get; set; }
        public IFormFile? File { get; set; }
        public int Read { get; set; } = 0;
        [StringLength(150)]
        public string Category { get; set; }
    }
}
