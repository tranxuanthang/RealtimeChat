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
    public class doiemailController : Controller
    {
        // GET: doiemail
        [HasCurrentUser]
        [HttpGet]
        public ActionResult doiemail()
        {
            User currentUser = (User)HttpContext.Items["currentUser"];//lay ng dung hien tai
            return View();
        }
        [HasCurrentUser]
        [HttpPost]
        public ActionResult doiemail(doiemail doiemail)
        {
            User currentUser = (User)HttpContext.Items["currentUser"];
            var db = new RealtimeChatDB();
            if (ModelState.IsValid)
            {
                    var a = db.Users.Attach(currentUser);
                    a.Email = doiemail.emailmoi;
                    a.ShowName = doiemail.namemoi;
                    db.SaveChanges();
                    TempData["ChangeInfoorPass"] = "Congratulations on Your Successful Information Exchange";
                    return RedirectToAction("Details", "Users");
            }
            else
            {
                    return View();
            }
        }
    }
}