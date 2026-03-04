using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bethel_Heritage_Academy_WebSite.Pages
{
    public class blogModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public PagedResult<NewsTable> List { get; set; }
        public blogModel(ApplicationDbContext db)
        {
            this._db = db;
        }
        public void OnGet(int p = 1)
        {
            var Data = _db.NewsTables.OrderByDescending(n => n.CreatedDate).ToList();

            var index = ToIndex(p);
            int pageSize = 9; // Show 9 articles per page (3x3 grid)
            List = new PagedResult<NewsTable>
            {
                Data = Data.Skip(index * pageSize).Take(pageSize).ToList(),
                PageSize = pageSize,
                PageNumber = p,
                TotalItems = Data.Count()
            };
        }
        private int ToIndex(int number)
        {
            var index = number - 1;
            return index < 0 ? 0 : index;
        }

        /// <summary>
        /// Truncates HTML content to specified length and adds ellipsis if truncated
        /// </summary>
        /// <param name="htmlContent">HTML content to truncate</param>
        /// <param name="maxLength">Maximum character length (default: 150)</param>
        /// <returns>Truncated text with ellipsis if needed</returns>
        
    }
}
