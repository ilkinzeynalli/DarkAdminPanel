using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DarkAdminPanel.WebUI.RequestInputModels
{
    public class LoginInputModel
    {
        [UIHint("email")]
        public string Email { get; set; }
        [UIHint("password")]
        public string Password { get; set; }
    }
}
