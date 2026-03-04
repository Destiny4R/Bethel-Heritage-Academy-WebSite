using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace Bethel_Heritage_Academy_WebSite.Pages
{
    public class managementModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public List<TeamManagement> team { get; set; }
        public managementModel(ApplicationDbContext db)
        {
            this._db = db;
        }
        public void OnGet()
        {
            team = _db.TeamManagements.OrderBy(k=>k.Hierarchy).Where(k=>k.Status).ToList();
        }
    }
}
