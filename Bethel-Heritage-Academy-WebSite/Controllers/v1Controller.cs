using Bethel_Heritage_Academy_WebSite.Models;
using Bethel_Heritage_Academy_WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bethel_Heritage_Academy_WebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class v1Controller : ControllerBase
    {
        private readonly IImageManager imageManager;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<v1Controller> logger;
        private readonly IWebHostEnvironment webHost;

        public v1Controller(IImageManager imageManager, ApplicationDbContext db, ILogger<v1Controller> logger, IWebHostEnvironment webHost)
        {
            this.imageManager = imageManager;
            this._db = db;
            this.logger = logger;
            this.webHost = webHost;
        }
        // GET: api/<v1Controller>
        [HttpPost("TeamManagements")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> TeamManagements([FromForm] TeamsInformation team)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (team.File != null)
                    {
                        string dataFileName = System.IO.Path.GetFileName(team.File.FileName);

                        string extension = System.IO.Path.GetExtension(dataFileName.ToLower());

                        string[] allowedExtsnions = new string[] { ".png", ".bitmap", ".jpg", ".jpeg" };

                        if (!allowedExtsnions.Contains(extension))

                        {
                            return Ok(new { success = false, message = "Sorry! the uploaded picture file is not in a picture or image format, only extension with png, jpg, jpeg, bmp, gif are allowed. Check and try again" });
                        }
                    }

                    if (team.Id == null || team.Id  < 1)
                    {
                        var theTeam = new TeamManagement
                        {
                            Fullname = team.Fullname,
                            Position = team.Position,
                            Email = team.Email,
                            Phone = team.Phone,
                            Note = team.Note,
                            Hierarchy = team.Hierarchy,
                            CreatedDate = DateTime.UtcNow,
                        };
                        if (team.File != null)
                        {
                            theTeam.ImagePath = await imageManager.CompressAndSaveImageAsync(team.File, @"\teams\");
                        }
                        _db.Add(theTeam);
                        int result = await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "Team successfully added" });
                    }
                    else
                    {
                        var theTeam = _db.TeamManagements.FirstOrDefault(x => x.Id == team.Id);
                        if (theTeam == null)
                        {
                            return Ok(new { success = false, message = "Sorry! the team information you want to update is not found in the database, check and try again" });
                        }
                        //Update the team information
                        theTeam.Fullname = team.Fullname;
                        theTeam.Position = team.Position;
                        theTeam.Email = team.Email;
                        theTeam.Phone = team.Phone;
                        theTeam.Note = team.Note;
                        theTeam.Hierarchy = team.Hierarchy;
                        theTeam.CreatedDate = DateTime.UtcNow;

                        if (team.File != null)
                        {
                            theTeam.ImagePath = await imageManager.CompressAndSaveImageAsync(team.File, @"\teams\", 120, theTeam.ImagePath);
                        }
                        _db.Update(theTeam);
                        int result = await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "Team successfully updated" });
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing the team management information.");
                    return Ok(new { success = false, message = "An error occurred while processing the team management information" });
                }
            }
            return Ok(new { success = false, message = "Some information are not complete, check and try again" });
        }

        // GET api/<v1Controller>/5
        [HttpDelete("TeamManagements/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id > 0)
            {
                try
                {
                    var theTeam = _db.TeamManagements.FirstOrDefault(x => x.Id == id);
                    if (theTeam == null)
                    {
                        return Ok(new { success = false, message = "Sorry! the team information you want to delete is not found in the database, check and try again" });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(theTeam.ImagePath))
                        {
                            string deleteOldImage = System.IO.Path.Combine(webHost.WebRootPath, theTeam.ImagePath);
                            if (System.IO.File.Exists(deleteOldImage))
                            {
                                System.IO.File.Delete(deleteOldImage);
                            }
                        }
                        _db.TeamManagements.Remove(theTeam);
                        int result = await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "Team information successfully deleted" });
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while deleting the team management information with id {id}.");
                }
            }
            return Ok(new { success = false, message = "Sorry! the team information you want to delete is not found in the database, check and try again" });
        }

        [HttpPut("TeamManagements/{id}")]
        public async Task<IActionResult> ToggledProfile(int id)
        {
            if (id > 0)
            {
                try
                {
                    var theTeam = _db.TeamManagements.FirstOrDefault(x => x.Id == id);
                    if (theTeam == null)
                    {
                        return Ok(new { success = false, message = "Sorry! the team information you want to delete is not found in the database, check and try again" });
                    }
                    else
                    {
                        if (theTeam.Status)
                        {
                            theTeam.Status = false;
                        }
                        else
                        {
                            theTeam.Status = true;
                        }
                        _db.TeamManagements.Update(theTeam);
                        int result = await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "Team member account successfully toggled" });
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while deleting the team management information with id {id}.");
                }
            }
            return Ok(new { success = false, message = "Sorry! the team information you want to delete is not found in the database, check and try again" });
        }



        [HttpPost("NewsManagements")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> NewsManagements([FromForm] NewsTableVM news)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (news.File != null)
                    {
                        string dataFileName = System.IO.Path.GetFileName(news.File.FileName);

                        string extension = System.IO.Path.GetExtension(dataFileName.ToLower());

                        string[] allowedExtsnions = new string[] { ".png", ".bitmap", ".jpg", ".jpeg" };

                        if (!allowedExtsnions.Contains(extension))

                        {
                            return Ok(new { success = false, message = "Sorry! the uploaded picture file is not in a picture or image format, only extension with png, jpg, jpeg, bmp, gif are allowed. Check and try again" });
                        }
                    }

                    if (news.Id == null || news.Id < 1)
                    {
                        var theTeam = new NewsTable
                        {
                            Reporter = news.Reporter,
                            Headline = news.Headline,
                            NewBody = news.NewBody,
                            Category = news.Category
                        };
                        if (news.File != null)
                        {
                            theTeam.ImagePath = await imageManager.CompressAndSaveImageAsync(news.File, @"\teams\");
                        }
                        _db.Add(theTeam);
                        int result = await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "News successfully added and posted" });
                    }
                    else
                    {
                        var theNews = _db.NewsTables.FirstOrDefault(x => x.Id == news.Id);
                        if (theNews == null)
                        {
                            return Ok(new { success = false, message = "Sorry! the team information you want to update is not found in the database, check and try again" });
                        }
                        //Update the team information
                        theNews.Reporter = news.Reporter;
                        theNews.Headline = news.Headline;
                        theNews.NewBody = news.NewBody;
                        theNews.Category = news.Category;

                        if (news.File != null)
                        {
                            theNews.ImagePath = await imageManager.CompressAndSaveImageAsync(news.File, @"\teams\", 120, theNews.ImagePath);
                        }
                        _db.Update(theNews);
                        int result = await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "News successfully updated and post" });
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing the news information.");
                    return Ok(new { success = false, message = "An error occurred while processing the news information" });
                }
            }
            return Ok(new { success = false, message = "Some information are not complete, check and try again" });
        }

        // GET api/<v1Controller>/5
        [HttpDelete("NewsManagements/{id}")]
        public async Task<IActionResult> NewsDelete(int id)
        {
            if (id > 0)
            {
                try
                {
                    var theTeam = _db.NewsTables.FirstOrDefault(x => x.Id == id);
                    if (theTeam == null)
                    {
                        return Ok(new { success = false, message = "Sorry! the news information you want to delete is not found in the database, check and try again" });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(theTeam.ImagePath))
                        {
                            string deleteOldImage = System.IO.Path.Combine(webHost.WebRootPath, theTeam.ImagePath);
                            if (System.IO.File.Exists(deleteOldImage))
                            {
                                System.IO.File.Delete(deleteOldImage);
                            }
                        }
                        _db.NewsTables.Remove(theTeam);
                        int result = await _db.SaveChangesAsync();
                        return Ok(new { success = true, message = "News information successfully deleted" });
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while deleting the news information with id {id}.");
                }
            }
            return Ok(new { success = false, message = "Sorry! news information you want to delete is not found in the database, check and try again" });
        }
    }
}
