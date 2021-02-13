using DarkAdminPanel.WebUI.Models.RequestInputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.ApiClients.Abstract
{
    public interface ITokenApiClient
    {
        Task<HttpResponseMessage> ValidateAsync(string token);
        Task<HttpResponseMessage> RefreshAsync(TokenApiInputModel model);
        Task<HttpResponseMessage> RevokeAsync();
    }
}
