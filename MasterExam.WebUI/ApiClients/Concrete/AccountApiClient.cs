using DarkAdminPanel.Core.Concrete.Models;
using DarkAdminPanel.Core.Concrete.RequestInputModels;
using DarkAdminPanel.Core.Concrete.ResponseOutputModels;
using DarkAdminPanel.WebUI.ApiClients.Abstract;
using DarkAdminPanel.WebUI.Extensions;
using DarkAdminPanel.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.ApiClients.Concrete
{
    public class AccountApiClient : IAccountApiClient
    {
        private readonly HttpClient _client;
        private readonly ILoginService _loginManager; 
        public AccountApiClient(HttpClient client,ILoginService loginManager)
        {
            _client = client;
            _loginManager = loginManager;
        }

        public async Task<HttpResponseMessage> GetUserByNameAsync(string userName)
        {
             HttpResponseMessage response = await _client.AddHeader()
                                                         .AddTokenToHeader(_loginManager.Token)
                                                         .GetAsync("/api/Account/GetUserByName/" + userName);

            return response;
        }
        public async Task<HttpResponseMessage> LoginAsync(LoginModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.AddHeader().PostAsync("/api/Account/Login", contentData);

            return response;
        }
        public async Task<HttpResponseMessage> RegisterAsync(RegisterModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.AddHeader()
                                                        .AddTokenToHeader(_loginManager.Token)
                                                        .PostAsync("/api/Account/Register", contentData);

            return response;
        }
        public async Task<HttpResponseMessage> ChangePasswordAsync(AccountSettingModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.AddHeader()
                                                    .AddTokenToHeader(_loginManager.Token)
                                                    .PutAsync("/api/Account/ChangePassword", contentData);

            return response;
        }

    }
}
