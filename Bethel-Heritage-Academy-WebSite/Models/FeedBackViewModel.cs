using System.ComponentModel.DataAnnotations;

namespace Bethel_Heritage_Academy_WebSite.Models
{
    public class FeedBackViewModel
    {
        [StringLength(100)]
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(550)]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }
    }
}
