using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealtimeChat.ViewModels;
using RealtimeChat.Models;
using System.Web.Security;

namespace RealtimeChat.Controllers
{
    public class SessionsController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginInfo)
        {
            if (ModelState.IsValid)
            {
                using (var db = new RealtimeChatDB())
                {
                    User loggingUser = db.Users.Where(user => user.UserName == loginInfo.UserName).FirstOrDefault();
                    if (loggingUser != null)
                    {
                        if (string.Equals(loginInfo.UserPassword, loggingUser.UserPassword))
                        {
                            FormsAuthentication.SetAuthCookie(loggingUser.UserName, loginInfo.RememberMe);
                            return RedirectToAction("Index", "Rooms");
                        }
                        else
                        {
                            ModelState.AddModelError("UserPassword", "Wrong password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "Username not found");
                    }
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Sessions");
        }
    }
}