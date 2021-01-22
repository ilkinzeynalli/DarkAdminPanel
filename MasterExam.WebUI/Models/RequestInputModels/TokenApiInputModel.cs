using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Models.RequestInputModels
{
    public class TokenApiInputModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
