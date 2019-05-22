namespace RealtimeChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ChatMessage
    {
        [Key]
        public int MessageID { get; set; }

        public int? SenderID { get; set; }

        public int? ToUserID { get; set; }

        public int? RoomID { get; set; }

        [StringLength(200)]
        public string MessageText { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Room Room { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
