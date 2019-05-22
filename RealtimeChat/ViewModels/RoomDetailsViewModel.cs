using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RealtimeChat.Models;

namespace RealtimeChat.ViewModels
{
    public class RoomDetailsViewModel
    {
        public Room CurrentRoom { get; set; }
        public List<RoomMember> RoomMembers { get; set; }
    }
}