using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RealtimeChat.ActionFilters;
using RealtimeChat.Models;
using RealtimeChat.ViewModels;

namespace RealtimeChat.Controllers
{
    public class RoomsController : Controller
    {
        // GET: Rooms
        [Authorize]
        [HasCurrentUser]
        public ActionResult Index()
        {
            List<Room> roomList;
            int currentUserID = ((User)HttpContext.Items["currentUser"]).UserID;
            using (var db = new RealtimeChatDB())
            {
                roomList = db.Rooms.Where(r => r.RoomMembers.Any(m => m.UserID == currentUserID)).ToList();
            }
            return View(roomList);
        }

        //GET: Rooms/All
        [Authorize]
        [HasCurrentUser]
        public ActionResult All()
        {
            List<Room> roomList;
            using (var db = new RealtimeChatDB())
            {
                roomList = db.Rooms.ToList();
            }
            ViewBag.currentUserID = ((User)HttpContext.Items["currentUser"]).UserID;
            return View(roomList);
        }

        [HttpPost]
        [Authorize]
        [HasCurrentUser]
        public ActionResult JoinRoom(int? roomID)
        {
            if (roomID != null)
            {
                int currentUserID = ((User)HttpContext.Items["currentUser"]).UserID;
                Room joiningRoom;
                using (var db = new RealtimeChatDB())
                {
                    joiningRoom = db.Rooms.Where(r => r.RoomID == roomID).FirstOrDefault();
                    if (joiningRoom != null)
                    {
                        RoomMember existingMember = db.RoomMembers.Where(m => m.UserID == currentUserID && m.RoomID == joiningRoom.RoomID).FirstOrDefault();
                        if (existingMember == null)
                        {
                            var newMember = db.RoomMembers.Create();
                            newMember.UserID = currentUserID;
                            newMember.RoomID = joiningRoom.RoomID;
                            newMember.MemberLevel = 0;
                            db.RoomMembers.Add(newMember);

                            db.SaveChanges();

                            TempData["successMessage"] = "You have joined the room successfully";
                            return RedirectToAction("All");
                        }
                        else
                        {
                            ModelState.AddModelError("JoinError", "You have already joined this room");
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("JoinError", "Room ID param was not found");
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        [HasCurrentUser]
        [HasJoinedRoom]
        public ActionResult LeaveRoom(int? id)
        {
            if (id != null)
            {
                int currentUserID = ((User)HttpContext.Items["currentUser"]).UserID;
                Room joiningRoom;
                using (var db = new RealtimeChatDB())
                {
                    joiningRoom = db.Rooms.Where(r => r.RoomID == id).FirstOrDefault();
                    if (joiningRoom != null)
                    {
                        RoomMember existingMember = db.RoomMembers.Where(m => m.UserID == currentUserID && m.RoomID == joiningRoom.RoomID).FirstOrDefault();
                        if (existingMember == null)
                        {
                            ModelState.AddModelError("LeaveError", "You are not joined this room");
                        }
                        else if (existingMember.MemberLevel == 2)
                        {
                            ModelState.AddModelError("LeaveError", "You cannot leave this room because you are the person who created it");
                        }
                        else
                        {
                            db.RoomMembers.Remove(existingMember);

                            db.SaveChanges();

                            TempData["successMessage"] = "You have leaved the room successfully";
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("JoinError", "Room ID param was not found");
            }
            return View();
        }

        // GET: Rooms/Details/5
        [Authorize]
        [HasCurrentUser]
        [HasJoinedRoom]
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                Room currentRoom;
                List<RoomMember> currentRoomMembers;
                var db = new RealtimeChatDB();
                currentRoom = db.Rooms.Where(r => r.RoomID == id).FirstOrDefault();
                currentRoomMembers = db.RoomMembers.Where(m => m.RoomID == currentRoom.RoomID).ToList();
                if (currentRoom != null)
                {
                    RoomDetailsViewModel roomDetails = new RoomDetailsViewModel
                    {
                        CurrentRoom = currentRoom,
                        RoomMembers = currentRoomMembers
                    };
                    return View(roomDetails);
                }
                else
                {
                    TempData["dangerMessage"] = "No room was matched with provided ID";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["dangerMessage"] = "Room ID parameter is not found";
                return RedirectToAction("Index");
            }
        }

        // GET: Rooms/Messages/5
        [Authorize]
        [HasCurrentUser]
        [HasJoinedRoom]
        public ActionResult Messages(int? id)
        {
            if (id != null)
            {
                Room currentRoom;
                using (var db = new RealtimeChatDB())
                {
                    currentRoom = db.Rooms.Where(r => r.RoomID == id).FirstOrDefault();
                }
                if (currentRoom != null)
                {
                    return View(currentRoom);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Rooms/Create
        [Authorize]
        [HasCurrentUser]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [Authorize]
        [HasCurrentUser]
        public ActionResult Create(RoomCreateViewModel roomCreateInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int currentUserID = ((User)HttpContext.Items["currentUser"]).UserID;
                    using (var db = new RealtimeChatDB())
                    {
                        var newRoom = db.Rooms.Create();
                        newRoom.RoomName = roomCreateInfo.RoomName;
                        newRoom.ShowName = roomCreateInfo.ShowName;
                        newRoom.RoomDescription = roomCreateInfo.Description;
                        newRoom.CreatorID = ((User)HttpContext.Items["currentUser"]).UserID;
                        db.Rooms.Add(newRoom);

                        var newMember = db.RoomMembers.Create();
                        newMember.UserID = ((User)HttpContext.Items["currentUser"]).UserID;
                        newMember.RoomID = newRoom.RoomID;
                        newMember.MemberLevel = 2;
                        db.RoomMembers.Add(newMember);

                        db.SaveChanges();

                        TempData["successMessage"] = "You have successfully created a room";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Rooms/Edit/5
        [Authorize]
        [HasCurrentUser]
        [HasJoinedRoom]
        [IsRoomAdmin]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [Authorize]
        [HasCurrentUser]
        [HasJoinedRoom]
        [IsRoomAdmin]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Rooms/Delete/5
        [Authorize]
        [HasCurrentUser]
        [HasJoinedRoom]
        [IsRoomAdmin]
        public ActionResult Delete(int id)
        {
            Room currentRoom = (Room)HttpContext.Items["currentRoom"];
            return View(currentRoom);
        }

        // POST: Rooms/Delete/5
        [HttpPost]
        [Authorize]
        [HasCurrentUser]
        [HasJoinedRoom]
        [IsRoomAdmin]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id, FormCollection collection)
        {
            // TODO: Add delete logic here
            User currentUser = (User)HttpContext.Items["currentUser"];
            Room currentRoom = (Room)HttpContext.Items["currentRoom"];

            using (var db = new RealtimeChatDB())
            {
                Room deleteRoom = db.Rooms.Attach(currentRoom);
                db.Rooms.Remove(deleteRoom);
                db.SaveChanges();
            }

            TempData["successMessage"] = "Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
