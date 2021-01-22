using DarkAdminPanel.WebUI.ApiClients.Abstract;
using DarkAdminPanel.WebUI.Extensions;
using DarkAdminPanel.WebUI.Models.RequestInputModels;
using DarkAdminPanel.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.ApiClients.Concrete
{
    public class TokenApiClient : ITokenApiClient
    {
        private readonly HttpClient _client;
        private readonly ILoginService _loginManager;

        public TokenApiClient(HttpClient client, ILoginService loginManager)
        {
            _client = client;
            _loginManager = loginManager;
        }

        public async Task<HttpResponseMessage> RefreshAsync(TokenApiInputModel model)
        {
            using (var httpClient = new HttpClient())
            {
                string data = JsonConvert.SerializeObject(model);
                var contentData = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.AddHeader().PostAsync("/api/token/refresh", contentData);

                return response;
            }
               
        }

        public async Task<HttpResponseMessage> ValidateAsync(string token)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.AddHeader()
                                                      .AddTokenToHeader(_loginManager.Token)
                                                      .GetAsync("/api/token/validate?token=" + token);

                return response;
            }
           

        }
    }
}
