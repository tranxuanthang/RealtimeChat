using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealtimeChat.ActionFilters;
using RealtimeChat.ViewModels;
using RealtimeChat.Models;
using System.Data.Entity.Validation;

namespace RealtimeChat.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RegisterViewModel registerInfo)
        {
            if (ModelState.IsValid)
            {
                using (var db = new RealtimeChatDB())
                {
                    var newUser = db.Users.Create();
                    newUser.UserName = registerInfo.UserName;
                    newUser.UserPassword = registerInfo.UserPassword;
                    newUser.Email = registerInfo.Email;
                    newUser.ShowName = registerInfo.ShowName;
                    newUser.CreatedAt = DateTime.Now;
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    TempData["successMessage"] = "Signed up successfully. Please login.";
                    return RedirectToAction("Login", "Sessions");
                }
            }
            else
            {
                return View();
            }
        }

        [HasCurrentUser]
        public ActionResult Details(int? id)
        {
            User currentUser = (User)HttpContext.Items["currentUser"];
            return View(currentUser);
        }
        [HasCurrentUser]
        [HttpGet]
        public ActionResult doimk()
        {
            User currentUser = (User)HttpContext.Items["currentUser"];//lay ng dung hien tai
            return View();
        }
        [HasCurrentUser]
        [HttpPost]
        public ActionResult doimk(doimk doimk)
        {
            User currentUser = (User)HttpContext.Items["currentUser"];
            var db = new RealtimeChatDB();
            if (ModelState.IsValid)
            {
                if (currentUser.UserPassword == doimk.mkcu)
                {
                    var a = db.Users.Attach(currentUser);
                    a.UserPassword = doimk.mkmoi;
                    db.SaveChanges();
                    TempData["ChangeInfoorPass"] = "Congratulations on Your Successful Password Exchange";
                    return RedirectToAction("Details", "Users");
                }
                else
                {
                    ModelState.AddModelError("mkcu", "Wrong Old Password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}