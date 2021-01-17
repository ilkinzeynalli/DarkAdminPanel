using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DarkAdminPanel.WebApi.ResponseOutputModels
{
    public class UserByNameGetOutputModel
    {
        public string UserId { get; set; }

        [UIHint("email")]
        public string UserName { get; set; }

        [UIHint("email")]
        public string Email { get; set; }
    }
}
