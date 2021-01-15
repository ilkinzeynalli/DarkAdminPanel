using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.ViewModels
{
    public class AccountSettingViewModel
    {
        public string Id { get; set; }

        [UIHint("email")]
        public string UserName { get; set; }

        [UIHint("email")]
        public string Email { get; set; }
        [UIHint("password")]
        public string Password { get; set; }
        [UIHint("password")]
        public string ConfirmPassword { get; set; }
    }
}
