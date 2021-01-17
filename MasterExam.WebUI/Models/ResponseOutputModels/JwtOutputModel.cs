using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.ResponseOutputModels
{
    public class JwtOutputModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
