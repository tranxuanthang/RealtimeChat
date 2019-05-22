using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtimeChat.ViewModels
{
    public class RoomMemberViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string ShowName { get; set; }
        public int MemberLevel { get; set; }
    }
}