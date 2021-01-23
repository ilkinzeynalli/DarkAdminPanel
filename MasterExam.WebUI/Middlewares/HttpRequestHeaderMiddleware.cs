using DarkAdminPanel.WebUI.ApiClients.Abstract;
using DarkAdminPanel.WebUI.Extensions;
using DarkAdminPanel.WebUI.Models.RequestInputModels;
using DarkAdminPanel.WebUI.Models.ResponseOutputModels;
using DarkAdminPanel.WebUI.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Middlewares
{
    public class HttpRequestHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenApiClient _tokenApiClient;
        private readonly ILoginService _loginManager;


        public HttpRequestHeaderMiddleware(RequestDelegate next, ITokenApiClient tokenApiClient, ILoginService loginManager)
        {
            _next = next;
            _tokenApiClient = tokenApiClient;
            _loginManager = loginManager;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!string.IsNullOrEmpty(_loginManager.Token))
            {
                var checkAccessToken = await _tokenApiClient.ValidateAsync(_loginManager.Token);
                var checkAccessTokenResult = await checkAccessToken.Content.ReadAsStringAsync();

                if (!Convert.ToBoolean(checkAccessTokenResult))
                {
                    var newTokensAsync = await _tokenApiClient.RefreshAsync(
                        new TokenApiInputModel() { 
                            AccessToken = _loginManager.Token,
                            RefreshToken = _loginManager.RefreshToken
                        }
                    );

                    if (newTokensAsync.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    {
                        var newTokensResult = await newTokensAsync.Content.ReadAsStringAsync();
                        var newTokens = JsonConvert.DeserializeObject<TokenOutputModel>(newTokensResult);

                        _loginManager.Token = newTokens.AccessToken;
                        _loginManager.RefreshToken = newTokens.RefreshToken;
                    }
                    else
                    {
                        _loginManager.Logout();
                    }
                }

                httpContext.Request.Headers.Add("Authorization", "Bearer " + _loginManager.Token);
            }

            await _next(httpContext); // calling next middleware
        }
    }
}
