using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace Bethel_Heritage_Academy_WebSite.Pages
{
    public class news_detailModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public NewsTable newsTable { get; set; }
        public news_detailModel(ApplicationDbContext db)
        {
            this._db = db;
        }
        public IActionResult OnGet(int id)
        {
            newsTable = _db.NewsTables.FirstOrDefault(n => n.Id == id);
            if (newsTable == null)
            {
                return RedirectToPage("blog");
            }
            return Page();
        }
    }
}
