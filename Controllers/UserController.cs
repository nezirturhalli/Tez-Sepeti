using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TezSepeti.Models;

namespace TezSepeti.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public UserModel User { get; set; }
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {

            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");

            }
            else
            {
                var jsonString = HttpContext.Session.GetString("user");
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
            }
            
            return View();
        }

        public IActionResult NewUser(int? id)
        {
            User = new UserModel();
            if (id == null)
            {
                //create
                return View(User);
            }
            //update
            User = _db.TezUsers.FirstOrDefault(u => u.ID == id);
            if (User == null)
            {
                return NotFound();
            }
            return View(User);
        }

        public IActionResult Login()
        {         
            return View();
        }

        public IActionResult Profile()
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");
            }
            else
            {
                var jsonString = HttpContext.Session.GetString("user");
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
            }
            return View();
        }
        public IActionResult ProfileDetail(int id)
        {
            UserModel UserInfo = new UserModel();
            if (HttpContext.Session.GetString("user") == null)
            {
                return RedirectToAction(actionName: "Login", controllerName: "User");
            }
            else
            {
                var jsonString = HttpContext.Session.GetString("user");
                UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);
                ViewBag.UserInfo = UserInfo;
                UserModel u = _db.TezUsers
                  .Where(u => u.ID == id).SingleOrDefault();
                ViewBag.UserDetail = u;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser()
        {
            if (ModelState.IsValid)
            {
                //create
                if (User.email.Contains(".edu"))
                {
                    User.userTypeID = 2;
                }              
                else
                {
                    User.userTypeID = 3;
                }
                User.createdDate = DateTime.Now;
                User.updatedDate = DateTime.Now;

                _db.TezUsers.Add(User);             
             
                _db.SaveChanges();

                return RedirectToAction(actionName: "Login", controllerName: "User");
            }
            return View(User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUser(String name, String lastName,String phonetemp,String password,IFormFile photo)
        {
            var jsonString = HttpContext.Session.GetString("user");
            UserModel UserInfo = JsonSerializer.Deserialize<UserModel>(jsonString);

            if (name != null)
                UserInfo.name = name;

            if (lastName != null)
                UserInfo.lastName = lastName;

            //if (phone != null)
            //    UserInfo.phone = phone;

            if (password != null)
                UserInfo.password = password;

            if (photo != null)
            {
                string imageExtension = Path.GetExtension(photo.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Uploads/{photo.FileName}");
                using var stream = new FileStream(path, FileMode.Create);
                photo.CopyToAsync(stream);
                UserInfo.imageName = photo.FileName;
            }

            _db.TezUsers.Update(UserInfo);


            _db.SaveChanges();

            ViewBag.UserInfo = UserInfo;
            return View("Profile");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginUser()
        {
            if (ModelState.IsValid)
            {
                //create
                var user = _db.TezUsers
                           .Where(s => s.email == User.email.ToString())
                           .Where(s => s.password == User.password.ToString())
                           .FirstOrDefault();
                
                if(user == null)
                {
                    return RedirectToAction(actionName: "Login", controllerName: "User");
                }
                else
                {
                    string jsonString = JsonSerializer.Serialize(user);
                    HttpContext.Session.SetString("user", jsonString);
                    return RedirectToAction("Index");

                }
            }
            return View();
        }


        public IActionResult Logout()
        {
                    HttpContext.Session.SetString("user", "");
                    return RedirectToAction(actionName: "Login", controllerName: "User");
        }

    }
}
