using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bethel_Heritage_Academy_WebSite.Models
{
    public class TeamsInformation
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [StringLength(100)]
        [JsonPropertyName("fullname")]
        public string Fullname { get; set; }
        [StringLength(60)]
        [JsonPropertyName("position")]
        public string Position { get; set; }
        [StringLength(100)]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [StringLength(11)]
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        public string Note { get; set; }
        [JsonPropertyName("file")]
        public IFormFile? File { get; set; }
        [JsonPropertyName("hierarchy")]
        public int Hierarchy { get; set; }
    }
}
