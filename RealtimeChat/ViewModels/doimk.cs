using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RealtimeChat.Models;

namespace RealtimeChat.ViewModels
{
    public class doimk
    {
        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "Password")]
        public string mkcu { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$")]
        public string mkmoi { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Compare("mkmoi", ErrorMessage = "Confirm password doesn't match")]
        [Display(Name = "Password Confirmation")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$")]
        public string xnmkmoi { get; set; }

    }
}