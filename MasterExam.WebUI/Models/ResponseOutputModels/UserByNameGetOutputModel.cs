using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DarkAdminPanel.WebUI.ResponseOutputModels
{
    public class UserByNameGetOutputModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
