using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DarkAdminPanel.WebUI.RequestInputModels
{
    public class RegisterInputModel
    {
        [UIHint("email")]
        [Required(ErrorMessage = "Istifadeci adini daxil edin")]
        [EmailAddress(ErrorMessage = "Istifadeci adini email formatinda daxil edin")]
        public string UserName { get; set; }
        [UIHint("email")]
        [Required(ErrorMessage = "Emaili daxil edin")]
        [EmailAddress(ErrorMessage = "Kecerli email daxil edin")]
        public string Email { get; set; }
        [UIHint("password")]
        [Required(ErrorMessage = "Passwordu daxil edin")]
        public string Password { get; set; }
    }
}
