using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.Models.ResponseOutputModels
{
    public class TokenApiOutputModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
