using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RealtimeChat.Models;

namespace RealtimeChat.ViewModels
{
    public class doiemail
    {
        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string emailmoi { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Show Name")]
        public string namemoi { get; set; }
    }
}