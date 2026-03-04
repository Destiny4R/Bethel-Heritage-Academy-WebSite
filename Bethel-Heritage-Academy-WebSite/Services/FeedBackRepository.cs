using Bethel_Heritage_Academy_WebSite.Models;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel.Models;

namespace Bethel_Heritage_Academy_WebSite.Services
{
    public interface IFeedBackRepository
    {
        Task<bool> SaveFeedBackAsync(FeedBack feedBack);
    }

    public class FeedBackRepository : IFeedBackRepository
    {
        private readonly ApplicationDbContext _context;
        
        public FeedBackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveFeedBackAsync(FeedBack feedBack)
        {
            try
            {
                _context.Add(feedBack);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

