using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealtimeChat.ViewModels
{
    public class SendMessageViewModel
    {
        [JsonProperty("UserID")]
        public int UserID { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("ShowName")]
        public string ShowName { get; set; }

        [JsonProperty("MessageText")]
        public string MessageText { get; set; }
    }
}