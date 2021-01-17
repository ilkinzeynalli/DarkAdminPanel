using DarkAdminPanel.WebUI.RequestInputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.ApiClients.Abstract
{
    public interface IAccountApiClient
    {
        Task<HttpResponseMessage> LoginAsync(LoginInputModel model);
        Task<HttpResponseMessage> RegisterAsync(RegisterInputModel model);
        Task<HttpResponseMessage> GetUserByNameAsync(string userEmail);
        Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordInputModel model);
    }
}
