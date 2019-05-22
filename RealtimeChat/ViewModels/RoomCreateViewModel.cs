using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using RealtimeChat.Models;

namespace RealtimeChat.ViewModels
{
    public class RoomCreateViewModel
    {
        [Required]
        [StringLength(50)]
        [UniqueRoomName]
        [Display(Name = "Room Name")]
        public string RoomName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Show Name")]
        public string ShowName { get; set; }

        [StringLength(255)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public class UniqueRoomNameAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                using (var db = new RealtimeChatDB())
                {
                    Room getRoom = db.Rooms.Where(room => room.RoomName == (string)value).FirstOrDefault();
                    if (getRoom != null)
                    {
                        return new ValidationResult("This room name has been taken");
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
            }
        }
    }
}