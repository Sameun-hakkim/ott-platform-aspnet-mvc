using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using OTTPlatform.DAL;
using OTTPlatform.Models;
using Login = OTTPlatform.Models.Login;

namespace OTTPlatform.Controllers
{
    public class VideoController : Controller
    {
        private readonly Video_DAL _DAL;
        private string connectionString = "YourConnectionStringHere";

        public VideoController(Video_DAL dal)
        {
            _DAL = dal;
        }
        [HttpGet]
        public IActionResult AdminPannel()
        {
            List<Category> categories = _DAL.GetCategories();
            ViewBag.Categories = categories;

            List<Language> Languages = _DAL.GetLanguage();
            ViewBag.Languages = Languages;

            List<Videos> videos = _DAL.GetVideoList();
            ViewBag.videos = videos;


            List<Login> login = _DAL.GetUserList();
            ViewBag.login = login;


            return View();
        }
        [HttpGet]
        public IActionResult VideoDisplay(int id, int logInID)
        {
            Videos videos = _DAL.GetVideoById(id);
            ViewBag.videos = videos;

            List<Comments> comments = _DAL.GetComments(id);

            ViewBag.comments = comments;

            if (logInID != 0)
            {
                var user = _DAL.GetLoginDetails(logInID);
                if (user != null)
                {
                    ViewBag.User = user;
                }
            }

            return View();
        }
        public IActionResult VideoComments(Comments comment)
        {
            bool result = _DAL.CommentVideo(comment);
            if (!result)
            {
                return Json(new { success = false, message = "Unable to update the comment." });
            }
           
            return Json(new { success = true, message = "Comments updated successfully!" });

        }
        [HttpGet]
        public IActionResult UserPannel(int logInID, string categoryName = null, string searchQuery = null)
        {
            if (logInID != null)
            {
                var user = _DAL.GetLoginDetails(logInID);
                if (user != null)
                {
                    ViewBag.User = user;
                }
            }

            List<Category> categories = _DAL.GetCategories();
            ViewBag.Categories = categories;

            List<Language> Languages = _DAL.GetLanguage();
            ViewBag.Languages = Languages;

            string IsUserPage = "yes";
            List<Videos> videos = _DAL.GetVideoList(categoryName, searchQuery, IsUserPage);
            ViewBag.videos = videos;

            ViewBag.SearchQuery = searchQuery;

            return View();
        }



        //      [HttpGet]
        //public IActionResult UserPannel(int logInID, string categoryName = null)
        //{
        //          if (logInID != null)
        //          {
        //              var user = _DAL.GetLoginDetails(logInID);
        //              if (user != null)
        //              {
        //                  ViewBag.User = user;
        //              }
        //          }

        //	List<Category> categories = _DAL.GetCategories();
        //	ViewBag.Categories = categories;

        //	List<Language> Languages = _DAL.GetLanguage();
        //	ViewBag.Languages = Languages;

        //          List<Videos> videos = _DAL.GetVideoList(categoryName); 
        //          ViewBag.videos = videos;



        //	return View();
        //}

        //[HttpPost]
        //public IActionResult AdminPannel([FromBody]Videos video, IFormFile VideoUpload)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        TempData["errorMessage"] = "Data Invalid";
        //    }
        //    if (VideoUpload != null && VideoUpload.Length > 0)
        //    {
        //        var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", VideoUpload.FileName);

        //        using (var stream = new FileStream(videoPath, FileMode.Create))
        //        {
        //            VideoUpload.CopyTo(stream);
        //        }

        //        video.VideoURL = Path.Combine("/uploads", VideoUpload.FileName);
        //    }
        //    bool result = _DAL.InsertVideo(video);
        //    if(!result)
        //    {
        //        TempData["errorMessage"] = "Unable to Save";
        //        ViewBag.Categories = _DAL.GetCategories();
        //        ViewBag.Languages = _DAL.GetLanguage();
        //        return View();
        //    }
        //    ViewBag.Categories = _DAL.GetCategories();
        //    ViewBag.Languages = _DAL.GetLanguage();


        //    return View();
        //}

        [HttpPost]
        public IActionResult AdminPannel(Videos video, IFormFile? VideoUpload, IFormFile? TemplateURL)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                TempData["errorMessage"] = "Data Invalid";
                ViewBag.Categories = _DAL.GetCategories();
                ViewBag.Languages = _DAL.GetLanguage();
                ViewBag.videos = _DAL.GetVideoList();
                ViewBag.login = _DAL.GetUserList(); 

                return View();
            }
            bool result = false;
            if (video.Id > 0)
            {
                //if (VideoUpload != null && VideoUpload.Length > 0)
                //{
                //    try
                //    {
                //        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                //        if (!Directory.Exists(uploadsFolder))
                //        {
                //            Directory.CreateDirectory(uploadsFolder);
                //        }

                //        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(VideoUpload.FileName);
                //        string filePath = Path.Combine(uploadsFolder, fileName);

                //        using (var stream = new FileStream(filePath, FileMode.Create))
                //        {
                //            VideoUpload.CopyTo(stream);
                //        }

                //        video.VideoURL = "/uploads/" + fileName;
                //    }
                //    catch (Exception ex)
                //    {
                //        TempData["errorMessage"] = "Error saving the video file: " + ex.Message;
                //        return View();
                //    }
                //}
                //if (TemplateURL != null && TemplateURL.Length > 0)
                //{
                //    try
                //    {
                //        string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates");
                //        if (!Directory.Exists(imgFolder))
                //        {
                //            Directory.CreateDirectory(imgFolder);
                //        }

                //        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(TemplateURL.FileName);
                //        string filePath = Path.Combine(imgFolder, fileName);

                //        using (var stream = new FileStream(filePath, FileMode.Create))
                //        {
                //            TemplateURL.CopyTo(stream);
                //        }

                //        video.TemplateURL = "/Templates/" + fileName;
                //    }
                //    catch (Exception ex)
                //    {
                //        TempData["errorMessage"] = "Error saving the Template file: " + ex.Message;
                //        return View();
                //    }
                //}
                result = _DAL.UpdateVideo(video);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to Save the video.";
                    return Json(new { success = false });
                }
                ViewBag.Categories = _DAL.GetCategories();
                ViewBag.Languages = _DAL.GetLanguage();
                ViewBag.videos = _DAL.GetVideoList();
                ViewBag.login = _DAL.GetUserList();

                return Json(new { success = true });
            }
            else
            {
                if (VideoUpload != null && VideoUpload.Length > 0)
                {
                    try
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(VideoUpload.FileName);
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            VideoUpload.CopyTo(stream);
                        }

                        video.VideoURL = "/uploads/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        TempData["errorMessage"] = "Error saving the video file: " + ex.Message;
                        return View();
                    }
                }
                if (TemplateURL != null && TemplateURL.Length > 0)
                {
                    try
                    {
                        string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates");
                        if (!Directory.Exists(imgFolder))
                        {
                            Directory.CreateDirectory(imgFolder);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(TemplateURL.FileName);
                        string filePath = Path.Combine(imgFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            TemplateURL.CopyTo(stream);
                        }

                        video.TemplateURL = "/Templates/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        TempData["errorMessage"] = "Error saving the Template file: " + ex.Message;
                        return View();
                    }

                }
                result = _DAL.InsertVideo(video);
                if (!result)
                {
                    TempData["errorMessage"] = "Unable to Save the video.";
                    return Json(new { success = false });
                }
                ViewBag.Categories = _DAL.GetCategories();
                ViewBag.Languages = _DAL.GetLanguage();
                ViewBag.videos = _DAL.GetVideoList();
                ViewBag.login = _DAL.GetUserList();

                return Json(new { success = true });
            }
          
            //else
            //{
            //    if (VideoUpload != null && VideoUpload.Length > 0)
            //    {
            //        try
            //        {
            //            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            //            if (!Directory.Exists(uploadsFolder))
            //            {
            //                Directory.CreateDirectory(uploadsFolder);
            //            }

            //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(VideoUpload.FileName);
            //            string filePath = Path.Combine(uploadsFolder, fileName);

            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                VideoUpload.CopyTo(stream);
            //            }

            //            video.VideoURL = "/uploads/" + fileName;
            //        }
            //        catch (Exception ex)
            //        {
            //            TempData["errorMessage"] = "Error saving the video file: " + ex.Message;
            //            return View();
            //        }
            //    }
            //    if (TemplateURL != null && TemplateURL.Length > 0)
            //    {
            //        try
            //        {
            //            string imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates");
            //            if (!Directory.Exists(imgFolder))
            //            {
            //                Directory.CreateDirectory(imgFolder);
            //            }

            //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(TemplateURL.FileName);
            //            string filePath = Path.Combine(imgFolder, fileName);

            //            using (var stream = new FileStream(filePath, FileMode.Create))
            //            {
            //                TemplateURL.CopyTo(stream);
            //            }

            //            video.TemplateURL = "/Templates/" + fileName;
            //        }
            //        catch (Exception ex)
            //        {
            //            TempData["errorMessage"] = "Error saving the Template file: " + ex.Message;
            //            return View();
            //        }
            //    }
              
           // }

            //return RedirectToAction("VideoList");
            return Json(new { success = false });
        }
        public IActionResult VideoDelete(int id)
        {
            bool result = _DAL.DeleteVideo(id);
            if (!result)
            {
                return Json(new { success = false, message = "Unable to delete the video." });
            }
            ViewBag.Categories = _DAL.GetCategories();
            ViewBag.Languages = _DAL.GetLanguage();
            ViewBag.videos = _DAL.GetVideoList();
            ViewBag.login = _DAL.GetUserList();


            return Json(new { success = true, message = "Video deleted successfully!" });

        }
        public IActionResult VideoUndo(int id)
        {
            bool result = _DAL.UndoVideo(id);
            if (!result)
            {
                return Json(new { success = false, message = "Unable to Retrive the video." });
            }
            ViewBag.Categories = _DAL.GetCategories();
            ViewBag.Languages = _DAL.GetLanguage();
            ViewBag.videos = _DAL.GetVideoList();
            ViewBag.login = _DAL.GetUserList();


            return Json(new { success = true, message = "Video Retrived successfully!" });

        }
        [HttpGet]
        public IActionResult GetVideoDetails(int id)
        {
            var video = _DAL.GetVideoById(id);

            if (video != null)
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = video.Id,
                        VideoName = video.VideoName,
                        VideoDescription = video.VideoDescription,
                        CategoryID = video.CategoryID,
                        LanguageID = video.LanguageID,
                        VideoURL = video.VideoURL,
                        TemplateURL = video.TemplateURL
                    }
                });
            }

            return Json(new { success = false, message = "Video not found" });
        }

        public IActionResult UserDelete(int id)
        {
            bool result = _DAL.InactiveUser(id);
            if (!result)
            {
                return Json(new { success = false, message = "Unable to Inactive the user." });
            }
            ViewBag.Categories = _DAL.GetCategories();
            ViewBag.Languages = _DAL.GetLanguage();
            ViewBag.videos = _DAL.GetVideoList();
            ViewBag.login = _DAL.GetUserList();

            return Json(new { success = true, message = "Inactivated successfully!" });

        }

        public IActionResult UserUndo(int id)
        {
            bool result = _DAL.ActiveUser(id);
            if (!result)
            {
                return Json(new { success = false, message = "Unable to Active the user." });
            }
            ViewBag.Categories = _DAL.GetCategories();
            ViewBag.Languages = _DAL.GetLanguage();
            ViewBag.videos = _DAL.GetVideoList();
            ViewBag.login = _DAL.GetUserList();

            return Json(new { success = true, message = "Activated successfully!" });

        }

        [HttpGet]
        public IActionResult VideoChat(int logInID, int CategoryId)
        {
            List<Category> categories = _DAL.GetCategories();
            ViewBag.Categories = categories;

            if (CategoryId == null || CategoryId == 0)
            {
                CategoryId = 1;
            }

            List<VideoMessage> videoMessages = _DAL.GetVideoMessage(CategoryId);
            ViewBag.videoMessages = videoMessages;
            ViewBag.CategoryID = CategoryId;

            if (logInID != 0)
            {
                var user = _DAL.GetLoginDetails(logInID);
                if (user != null)
                {
                    ViewBag.User = user;
                }
            }
            ViewBag.cateName = _DAL.GetCategoryName(CategoryId);

            return View();
        }
        [HttpGet]
        public JsonResult GetMessages(int CategoryId)
        {
            return Json(_DAL.GetVideoMessage(CategoryId));
        }

        [HttpPost]
        public IActionResult ChatInsert(VideoMessage message)
        {
            var chat = _DAL.InsertChat(message);

            if (chat != null)
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = message.Id,
                        UserId = message.UserId,
                        CategoryId = message.CategoryId,
                        UserMessage = message.UserMessage,
                        Username = message.Username
                    }
                });
            }

            return Json(new { success = false, message = "Message failed" });
        }


    }
}
