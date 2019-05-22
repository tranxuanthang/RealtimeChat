using RealtimeChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace RealtimeChat.ActionFilters
{
    public class HasCurrentUser : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                using (var db = new RealtimeChatDB())
                {
                    string currentUserName = filterContext.HttpContext.User.Identity.Name;
                    User currentUser = db.Users.Where(u => u.UserName == currentUserName).FirstOrDefault();
                    if (currentUser != null)
                    {
                        filterContext.HttpContext.Items["currentUser"] = currentUser;
                    }
                    else
                    {
                        FormsAuthentication.SignOut();
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { action = "Login", controller = "Sessions" })
                        );
                    }
                }
            }
        }
    }

    public class HasJoinedRoom : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Room currentRoom;
            User currentUser = (User)filterContext.HttpContext.Items["currentUser"];
            int currentRoomID = Convert.ToInt32(filterContext.RouteData.Values["id"]);
            using (var db = new RealtimeChatDB())
            {
                currentRoom = db.Rooms.Where(r => r.RoomID == currentRoomID).FirstOrDefault();
                if (currentRoom != null)
                {
                    RoomMember existingMember = db.RoomMembers.Where(m => m.UserID == currentUser.UserID && m.RoomID == currentRoom.RoomID).FirstOrDefault();
                    if (existingMember == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { action = "Index", controller = "Rooms" })
                        );
                    }
                    else
                    {
                        filterContext.HttpContext.Items["currentRoom"] = currentRoom;
                    }
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { action = "Index", controller = "Rooms" })
                    );
                }
            }
        }
    }

    public class IsRoomAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Room currentRoom = (Room)filterContext.HttpContext.Items["currentRoom"];
            User currentUser = (User)filterContext.HttpContext.Items["currentUser"];
            if (currentRoom != null)
            {
                int currentRoomID = currentRoom.RoomID;
                int currentUserID = currentUser.UserID;
                using (var db = new RealtimeChatDB())
                {
                    RoomMember currentRoomMember = db.RoomMembers.Where(m => m.RoomID == currentRoomID && m.UserID == currentUserID).FirstOrDefault();
                    if (currentRoomMember.MemberLevel < 2)
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new { action = "Index", controller = "Rooms" })
                        );
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { action = "Index", controller = "Rooms" })
                );
            }
        }
    }
}