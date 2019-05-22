using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using RealtimeChat.Models;
using RealtimeChat.ViewModels;

namespace RealtimeChat.Hubs
{
    [Authorize]
    public class MessagesHub : Hub
    {
        private static readonly ConcurrentDictionary<string, int> joinedUsers = new ConcurrentDictionary<string, int>();

        public void Send(string message)
        {
            User currentUser;
            Room currentRoom;
            int currentRoomID = joinedUsers[Context.ConnectionId];
            using (var db = new RealtimeChatDB())
            {
                currentUser = db.Users.Where(u => u.UserName == Context.User.Identity.Name).FirstOrDefault();
                currentRoom = db.Rooms.Where(r => r.RoomID == currentRoomID).FirstOrDefault();
                var newMessage = db.ChatMessages.Create();
                newMessage.SenderID = currentUser.UserID;
                newMessage.RoomID = currentRoom.RoomID;
                newMessage.MessageText = message;
                db.ChatMessages.Add(newMessage);
                db.SaveChanges();

                Clients.Group("Room_" + joinedUsers[Context.ConnectionId]).broadcastMessage(
                    new SendMessageViewModel
                    {
                        UserID = currentUser.UserID,
                        UserName = currentUser.UserName,
                        ShowName = currentUser.ShowName,
                        MessageText = newMessage.MessageText
                    }
                );
            }
        }

        public void Join(string roomID)
        {
            Groups.Add(Context.ConnectionId, "Room_" + roomID);
        }

        public void Hello()
        {
            Clients.All.hello();
        }

        public override Task OnConnected()
        {
            string roomIDstr = Context.QueryString["roomID"];
            int roomID = Convert.ToInt32(Context.QueryString["roomID"]);
            Groups.Add(Context.ConnectionId, "Room_" + roomID);
            joinedUsers.GetOrAdd(Context.ConnectionId, roomID);

            using (var db = new RealtimeChatDB())
            {
                List<ChatMessage> initMessages;
                List<SendMessageViewModel> formattedMessages = new List<SendMessageViewModel>();
                initMessages = db.ChatMessages.Where(msg => msg.RoomID == roomID).ToList();
                initMessages.ForEach(msg =>
                {
                    User sender = db.Users.Where(u => u.UserID == msg.SenderID).FirstOrDefault();
                    formattedMessages.Add(new SendMessageViewModel
                    {
                        UserID = sender.UserID,
                        UserName = sender.UserName,
                        ShowName = sender.ShowName,
                        MessageText = msg.MessageText
                    });
                });
                Clients.Caller.broadcastMessages(formattedMessages);
            }
            return base.OnConnected();
        }
    }
}