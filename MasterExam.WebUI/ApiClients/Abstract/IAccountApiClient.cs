using DarkAdminPanel.Core.Concrete.RequestInputModels;
using DarkAdminPanel.Core.Concrete.ResponseOutputModels;
using DarkAdminPanel.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.ApiClients.Abstract
{
    public interface IAccountApiClient
    {
        Task<HttpResponseMessage> LoginAsync(LoginModel model);
        Task<HttpResponseMessage> RegisterAsync(RegisterModel model);
        Task<HttpResponseMessage> GetUserByNameAsync(string userEmail);
        Task<HttpResponseMessage> ChangePasswordAsync(AccountSettingModel model);
    }
}
