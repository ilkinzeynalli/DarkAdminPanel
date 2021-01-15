using DarkAdminPanel.WebUI.Extensions;
using DarkAdminPanel.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Services.Concrete
{
    public class LoginManager : ILoginService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public LoginManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Token
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session.GetJson<string>("JWToken");
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _httpContextAccessor.HttpContext.Session.SetJson("JWToken", value);
                }
            }
        }
        public string UserName => _httpContextAccessor.HttpContext.User.Identity.Name;

        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.SetJson("JWToken", null);
        }
    }
}
