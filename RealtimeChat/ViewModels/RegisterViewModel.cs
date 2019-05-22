using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using RealtimeChat.Models;

namespace RealtimeChat.ViewModels
{
    [NotMapped]
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        [RegularExpression(@"^[A-Za-z0-9_.]+$")]
        [UniqueUserName]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [Compare("UserPassword", ErrorMessage = "Confirm password doesn't match")]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirmation { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Show Name")]
        public string ShowName { get; set; }

        public class UniqueUserNameAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                using (var db = new RealtimeChatDB())
                {
                    User getUser = db.Users.Where(user => user.UserName == (string)value).FirstOrDefault();
                    if (getUser != null)
                    {
                        return new ValidationResult("This username has been taken");
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