namespace RealtimeChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            ChatMessages = new HashSet<ChatMessage>();
            RoomMembers = new HashSet<RoomMember>();
        }

        public int RoomID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomName { get; set; }

        [StringLength(50)]
        public string ShowName { get; set; }

        public int? CreatorID { get; set; }

        [Column(TypeName = "text")]
        public string RoomDescription { get; set; }

        public DateTime? CreatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomMember> RoomMembers { get; set; }

        public virtual User User { get; set; }

        public bool IsUserJoined(int userID)
        {
            using (var db = new RealtimeChatDB())
            {
                RoomMember existingMember = db.RoomMembers.Where(m => m.UserID == userID && m.RoomID == RoomID).FirstOrDefault();
                if (existingMember != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
