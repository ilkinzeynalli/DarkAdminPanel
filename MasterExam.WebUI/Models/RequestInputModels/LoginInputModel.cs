using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DarkAdminPanel.WebUI.RequestInputModels
{
    public class LoginInputModel
    {
        [UIHint("email")]
        [Required(ErrorMessage = "Email daxil edin")]
        public string Email { get; set; }
        [UIHint("password")]
        [Required(ErrorMessage = "Passwordu daxil edin")]
        public string Password { get; set; }
    }
}
