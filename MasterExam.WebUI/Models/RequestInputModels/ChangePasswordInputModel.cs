using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DarkAdminPanel.WebUI.RequestInputModels
{
    public class ChangePasswordInputModel
    {
        public string UserId { get; set; }

        [UIHint("email")]
        public string UserName { get; set; }

        [UIHint("email")]
        public string Email { get; set; }

        [UIHint("password")]
        [Required(ErrorMessage = "Passwordu daxil edin")]
        public string Password { get; set; }

        [UIHint("password")]
        [Required(ErrorMessage = "ConfirmPassword daxil edin")]
        [Compare(nameof(Password),ErrorMessage = "Passwordlar uygun gelmir")]
        public string ConfirmPassword { get; set; }
    }
}
