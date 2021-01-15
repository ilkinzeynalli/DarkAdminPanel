using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Services.Abstract
{
    public interface ILoginService
    {
        public string Token { get; set; }
        public string UserName { get; }

        public void Logout();
    }
}
