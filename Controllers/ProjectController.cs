using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TezSepeti.Models;

namespace TezSepeti.Controllers
{
    public class ProjectController : Controller
    {
        private IHostingEnvironment Environment;

        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ProjectModel projectModel { get; set; }
        public ProjectController(ApplicationDbContext db, IHostingEnvironment _environment)
        {
            Environment = _environment;
            _db = db;
        }

        public IActionResult Index()
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            var jsonString = HttpContext.Session.GetString("user");
            if(jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;                
            }
            return View();
        }

        public IActionResult NewProject()
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            var jsonString = HttpContext.Session.GetString("user");
            if (jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
                return View();
            }


            return RedirectToAction("Login", "User");
        }

        public IActionResult MyProjects()
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            var jsonString = HttpContext.Session.GetString("user");
            if (jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
                List<ProjectModel> projects = _db.TezProjects
                  .Where(p=> p.userID == UserInfo.ID) 
                  .ToList();
                ViewBag.MyProjects = projects;
                return View();
            }
            return RedirectToAction("Login", "User");
        }

        public IActionResult ListProjects()
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            var jsonString = HttpContext.Session.GetString("user");
            if (jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
                List<ProjectModel> projects = _db.TezProjects
                  .ToList();

                List<FavLikeViewModel> pDetail = new List<FavLikeViewModel>();                
                foreach (var item in projects)
                {
                    FavLikeViewModel x = new FavLikeViewModel();

                    x.ID = item.ID;

                    int check = _db.TezFav
                        .Where(p => p.projectID == item.ID && p.userID == UserInfo.ID)
                        .Count();
                    x.faved = (check > 0) ? "true" : "false";

                    x.favCount = _db.TezFav
                       .Where(p => p.projectID == item.ID)
                       .Count();
                    x.favCount -= check;
                    check = _db.TezLike
                         .Where(p => p.projectID == item.ID && p.userID == UserInfo.ID)
                         .Count();
                    x.liked = (check>0)?"true":"false";

                    x.likeCount = _db.TezLike
                       .Where(p => p.projectID == item.ID)
                       .Count();
                    x.likeCount -= check;
                    x.viewed = item.viewed;
                    x.p = item;
                    List<MediaModel> mList = JsonSerializer.Deserialize<List<MediaModel>>(item.mediaJson);
                    MediaModel m = mList.FirstOrDefault();
                    x.img = "/Uploads/" + item.ID + "/Media/" + m.FileName;
                    pDetail.Add(x);
                }
                ViewBag.MyProjects = projects;
                ViewBag.pDetail = pDetail;
                return View();
            }


            return RedirectToAction("Login", "User");
        }

        public IActionResult FavProjects()
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            var jsonString = HttpContext.Session.GetString("user");
            if (jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
                List<int> favorites = _db.TezFav                  
                  .Where(f=> f.userID == UserInfo.ID)
                  .Select(f => f.projectID)
                  .ToList();
                
                List<ProjectModel> projects = _db.TezProjects
                  .Where(f=>favorites.Contains(f.ID))
                  .ToList();

                List<FavLikeViewModel> pDetail = new List<FavLikeViewModel>();
                foreach (var item in projects)
                {
                    FavLikeViewModel x = new FavLikeViewModel();

                    x.ID = item.ID;

                    int check = _db.TezFav
                        .Where(p => p.projectID == item.ID && p.userID == UserInfo.ID)
                        .Count();
                    x.faved = (check > 0) ? "true" : "false";

                    x.favCount = _db.TezFav
                       .Where(p => p.projectID == item.ID)
                       .Count();
                    x.favCount -= check;
                    check = _db.TezLike
                         .Where(p => p.projectID == item.ID && p.userID == UserInfo.ID)
                         .Count();
                    x.liked = (check > 0) ? "true" : "false";

                    x.likeCount = _db.TezLike
                       .Where(p => p.projectID == item.ID)
                       .Count();
                    x.likeCount -= check;
                    x.viewed = item.viewed;
                    List<MediaModel> mList = JsonSerializer.Deserialize<List<MediaModel>>(item.mediaJson);
                    MediaModel m = mList.First();
                    x.img = "/Uploads/" + item.ID + "/Media/" + m.FileName;
                    x.p = item;
                    pDetail.Add(x);
                }
                ViewBag.MyProjects = projects;
                ViewBag.pDetail = pDetail;
                return View();
            }


            return RedirectToAction("Login", "User");
        }

        public IActionResult SearchProject(string s)
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            var jsonString = HttpContext.Session.GetString("user");
            if (jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
                List<ProjectModel> projects = _db.TezProjects
                    .Where(p=> p.tags.Contains(s) || p.subject.Contains(s) || p.description.Contains(s))                  
                    .ToList();
                ViewBag.MyProjects = projects;
                return View();
            }


            return RedirectToAction("Login", "User");
        }

        public IActionResult Edit(int id)
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            var jsonString = HttpContext.Session.GetString("user");
            if (jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
                ProjectModel project = _db.TezProjects
                  .Where(p => p.ID == id).SingleOrDefault();           
                ViewBag.mediaList = JsonSerializer.Deserialize<List<MediaModel>>(project.mediaJson);
                ViewBag.fileList = JsonSerializer.Deserialize<List<MediaModel>>(project.fileJson);
                ViewBag.textList = JsonSerializer.Deserialize<List<TextModel>>(project.textJson);
                ViewBag.project = project;
                return View();
            }


            return RedirectToAction("Login", "User");
        }

        public IActionResult SingleProject(int id)
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            var jsonString = HttpContext.Session.GetString("user");
            if (jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
                ProjectModel project = _db.TezProjects
                  .Where(p => p.ID == id).SingleOrDefault();
                UserModel u = _db.TezUsers
                  .Where(p => p.ID == project.userID).SingleOrDefault();
                project.viewed++;
                ViewBag.mediaList = JsonSerializer.Deserialize<List<MediaModel>>(project.mediaJson);
                ViewBag.fileList = JsonSerializer.Deserialize<List<MediaModel>>(project.fileJson);
                ViewBag.textList = JsonSerializer.Deserialize<List<TextModel>>(project.textJson);
                ViewBag.project = project;
                ViewBag.puser = u;
                _db.TezProjects.Update(project);
                _db.SaveChanges();
                return View();
            }

            
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public string FavoriteFunc(int ProjectID)
        {
            UserModel UserInfo = new UserModel();
            var jsonString = HttpContext.Session.GetString("user");
            UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
            ViewBag.UserInfo = UserInfo;

            FavModel check = _db.TezFav
                        .Where(p=> p.projectID == ProjectID && p.userID == UserInfo.ID)
                        .FirstOrDefault();
            int count = _db.TezFav
                        .Where(p => p.projectID == ProjectID)
                        .Count();
            if (check != null)
            {
                _db.TezFav.Remove(check);
                _db.SaveChanges();
                return "false,";
            }
            else
            {
                FavModel x = new FavModel();
                x.projectID = ProjectID;
                x.userID = UserInfo.ID;
                x.createdDate = DateTime.Now;
               
                _db.TezFav.Add(x);
                _db.SaveChanges();                
                return "true";
            }
        }

        [HttpPost]
        public string LikeFunc(int ProjectID)
        {
            UserModel UserInfo = new UserModel();
            var jsonString = HttpContext.Session.GetString("user");
            UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
            ViewBag.UserInfo = UserInfo;

            LikeModel check = _db.TezLike
                        .Where(p => p.projectID == ProjectID && p.userID == UserInfo.ID)
                        .FirstOrDefault();
            int count = _db.TezLike
                        .Where(p => p.projectID == ProjectID)
                        .Count();
            if (check != null)
            {
                _db.TezLike.Remove(check);
                _db.SaveChanges();
                return "false,";
            }
            else
            {
                LikeModel x = new LikeModel();
                x.projectID = ProjectID;
                x.userID = UserInfo.ID;
                x.createdDate = DateTime.Now;

                _db.TezLike.Add(x);
                _db.SaveChanges();
                return "true";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(IFormCollection formFields)
        {
            UserModel UserInfo = new UserModel();
            var jsonString = HttpContext.Session.GetString("user");
            if (jsonString != null)
            {
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
            }
            ProjectModel pmodel = new ProjectModel();
            pmodel.subject = formFields["subject"];
            pmodel.totalValue = Convert.ToDecimal(formFields["total_value"]);
            pmodel.currentValue = Convert.ToDecimal(formFields["current_value"]);
            pmodel.personelCount = Convert.ToInt32(formFields["personel_count"]);
            pmodel.time = formFields["time"];
            pmodel.tags = formFields["ptags"];
            pmodel.supportersList = formFields["supporters_list"];
            pmodel.description = formFields["description"];
            pmodel.userID = UserInfo.ID;
            pmodel.createdDate = DateTime.Now;
            _db.TezProjects.Add(pmodel);
            _db.SaveChanges();
            string s = formFields["media_list"];
            string[] media_list = s.Split(',');
            List<MediaModel> mediaList = new List<MediaModel>();
            foreach (var item in media_list)
            {
                IFormFile formFile = formFields.Files[item];
                if (formFile != null)
                    if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    
                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;

                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads/" + pmodel.ID + "/Media/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var filePath = path + formFile.FileName;
                    MediaModel m = new MediaModel();
                    m.Name = item;
                    m.FileName = formFile.FileName;
                    m.Path = filePath;
                    mediaList.Add(m);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            pmodel.mediaJson = JsonSerializer.Serialize(mediaList);
            s = formFields["file_list"];
            string[] file_list = s.Split(',');
            List<MediaModel> fileList = new List<MediaModel>();
            foreach (var item in file_list)
            {
                IFormFile formFile = formFields.Files[item];
                if (formFile != null)
                    if (formFile.Length > 0)
                {
                    // full path to file in temp location                   
                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;

                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads/" + pmodel.ID + "/File/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var filePath = path + formFile.FileName;
                    MediaModel m = new MediaModel();
                    m.Name = item;
                    m.FileName = formFile.FileName;
                    m.Path = filePath;
                    fileList.Add(m);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            pmodel.fileJson = JsonSerializer.Serialize(fileList);
            s = formFields["text_list"];
            string[] text_list = s.Split(',');
            List<TextModel> textList = new List<TextModel>();
            foreach (var item in text_list)
            {
                
                               
                  
                    TextModel m = new TextModel();
                    m.SubjectName = "tsubject" + item.Substring(4);
                    m.SubjectText = formFields["tsubject" + item.Substring(4)];
                    m.DescptionName = item;
                    m.DescptionText = formFields[item];               
                    textList.Add(m);
               
                
            }
            pmodel.textJson = JsonSerializer.Serialize(textList);

            _db.TezProjects.Update(pmodel);


            _db.SaveChanges();

            return RedirectToAction(actionName: "MyProjects", controllerName: "Project");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProject(IFormCollection formFields)
        {
            
            ProjectModel pmodel = new ProjectModel();
            pmodel.ID = Convert.ToInt32(formFields["pID"]);
            ProjectModel project = _db.TezProjects.Where(p => p.ID == pmodel.ID).FirstOrDefault();
            pmodel = project;
            pmodel.subject = formFields["subject"];
            pmodel.totalValue = Convert.ToDecimal(formFields["total_value"]);
            pmodel.currentValue = Convert.ToDecimal(formFields["current_value"]);
            pmodel.personelCount = Convert.ToInt32(formFields["personel_count"]);
            pmodel.time = formFields["time"];
            pmodel.tags = formFields["ptags"];
            pmodel.supportersList = formFields["supporters_list"];
            pmodel.description = formFields["description"];
            pmodel.updatedDate = DateTime.Now;
            string s = formFields["media_list"];
            List<string> media_list = new List<string>();
            foreach (var item in s.Split(','))
                media_list.Add(item);
            List<MediaModel> mediaList = JsonSerializer.Deserialize<List<MediaModel>>(project.mediaJson);
            foreach (var item in mediaList)
                media_list.Remove(item.Name);
            foreach (var item in media_list)
            {
                IFormFile formFile = formFields.Files[item];
                if (formFile != null)
                    if (formFile.Length > 0)
                    {
                        // full path to file in temp location
                        UserModel UserInfo = new UserModel();
                        var jsonString = HttpContext.Session.GetString("user");
                        if (jsonString != null)
                        {
                            UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                            ViewBag.UserInfo = UserInfo;
                        }
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path = Path.Combine(this.Environment.WebRootPath, "Uploads/" + pmodel.ID + "/Media/");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var filePath = path + formFile.FileName;
                        if (!mediaList.Any(x => x.Name == formFile.FileName))
                        {
                            MediaModel m = new MediaModel();
                            m.Name = item;
                            m.FileName = formFile.FileName;
                            m.Path = filePath;
                            mediaList.Add(m);
                        }
                        else
                        {
                            int index = mediaList.FindIndex(item => item.Name == formFile.FileName);
                            mediaList[index].Name = item;
                            mediaList[index].FileName = formFile.FileName;
                            mediaList[index].Path = filePath;                            
                        }
                        
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
            }
            pmodel.mediaJson = JsonSerializer.Serialize(mediaList);
            s = formFields["file_list"];
            string[] file_list = s.Split(',');
            List<MediaModel> fileList = JsonSerializer.Deserialize<List<MediaModel>>(project.fileJson);
            foreach (var item in file_list)
            {
                IFormFile formFile = formFields.Files[item];
                if (formFile != null)
                    if (formFile.Length > 0)
                    {
                        // full path to file in temp location
                        UserModel UserInfo = new UserModel();
                        var jsonString = HttpContext.Session.GetString("user");
                        if (jsonString != null)
                        {
                            UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                            ViewBag.UserInfo = UserInfo;
                        }
                        string wwwPath = this.Environment.WebRootPath;
                        string contentPath = this.Environment.ContentRootPath;

                        string path = Path.Combine(this.Environment.WebRootPath, "Uploads/" + pmodel.ID + "/File/");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        /*var filePath = path + formFile.FileName;
                        MediaModel m = new MediaModel();
                        m.Name = item;
                        m.FileName = formFile.FileName;
                        m.Path = filePath;
                        fileList.Add(m);*/

                        var filePath = path + formFile.FileName;
                        if (!fileList.Any(x => x.Name == formFile.FileName))
                        {
                            MediaModel m = new MediaModel();
                            m.Name = item;
                            m.FileName = formFile.FileName;
                            m.Path = filePath;
                            fileList.Add(m);
                        }
                        else
                        {
                            int index = fileList.FindIndex(item => item.Name == formFile.FileName);
                            fileList[index].Name = item;
                            fileList[index].FileName = formFile.FileName;
                            fileList[index].Path = filePath;
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
            }
            pmodel.fileJson = JsonSerializer.Serialize(fileList);
            s = formFields["text_list"];
            string[] text_list = s.Split(',');
            List<TextModel> textList = new List<TextModel>();
            foreach (var item in text_list)
            {

                TextModel m = new TextModel();
                m.SubjectName = "tsubject" + item.Substring(4);
                m.SubjectText = formFields["tsubject" + item.Substring(4)];
                m.DescptionName = item;
                m.DescptionText = formFields[item];
                textList.Add(m);
            }
            pmodel.textJson = JsonSerializer.Serialize(textList);

            _db.TezProjects.Update(pmodel);


            _db.SaveChanges();

            return RedirectToAction("Edit", "Project", new { id = pmodel.ID });
        }

    

    }
}
