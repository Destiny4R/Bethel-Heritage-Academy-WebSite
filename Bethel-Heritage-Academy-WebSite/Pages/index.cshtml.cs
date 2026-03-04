using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Bethel_Heritage_Academy_WebSite.Models;
using Bethel_Heritage_Academy_WebSite.Services;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.Models;

namespace Bethel_Heritage_Academy_WebSite.Pages
{
    public class indexModel : PageModel
    {
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly IRecaptchaService _recaptchaService;
        private readonly ApplicationDbContext _db;

        public List<NewsTable> newsTable { get; set; } = new List<NewsTable>();
        public indexModel(IFeedBackRepository feedBackRepository, IRecaptchaService recaptchaService, ApplicationDbContext db)
        {
            _feedBackRepository = feedBackRepository;
            _recaptchaService = recaptchaService;
            this._db = db;
        }

        [BindProperty]
        public FeedBackViewModel FeedBack { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
            newsTable = _db.NewsTables.OrderByDescending(n => n.CreatedDate).Take(3).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate reCAPTCHA first
            var recaptchaResponse = Request.Form["g-recaptcha-response"];
            if (string.IsNullOrEmpty(recaptchaResponse))
            {
                StatusMessage = "Error: Please complete the reCAPTCHA verification.";
                return Page();
            }

            var isRecaptchaValid = await _recaptchaService.ValidateRecaptchaAsync(recaptchaResponse);
            if (!isRecaptchaValid)
            {
                StatusMessage = "Error: reCAPTCHA validation failed. Please try again.";
                return Page();
            }

            // Validate model state
            if (!ModelState.IsValid)
            {
                StatusMessage = "Error: Please fill in all required fields correctly.";
                return Page();
            }

            try
            {
                // Map ViewModel to Entity
                var feedback = new FeedBack
                {
                    Name = FeedBack.Name,
                    Subject = FeedBack.Subject,
                    Email = FeedBack.Email,
                    Message = FeedBack.Message,
                    IsRead = false,
                    CreatedDate = DateTime.Now
                };

                // Save to database using repository
                var success = await _feedBackRepository.SaveFeedBackAsync(feedback);

                if (success)
                {
                    StatusMessage = "Success: Thank you for contacting us! We'll get back to you soon.";
                    return RedirectToPage();
                }
                else
                {
                    StatusMessage = "Error: An error occurred while sending your message. Please try again.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: An error occurred while sending your message. Please try again.";
                // Log the exception here
                return Page();
            }
        }
    }
}
