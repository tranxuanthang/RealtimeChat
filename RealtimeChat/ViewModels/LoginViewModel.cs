using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RealtimeChat.Models;

namespace RealtimeChat.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}