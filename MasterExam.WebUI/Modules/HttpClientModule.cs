using DarkAdminPanel.WebUI.ApiClients.Abstract;
using DarkAdminPanel.WebUI.ApiClients.Concrete;
using DarkAdminPanel.WebUI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Modules
{
    public static class HttpClientModule
    {
        public static void Load(IServiceCollection services)
        {
            services.AddScoped<IAccountApiClient, AccountApiClient>();
            services.AddScoped<ITokenApiClient, TokenApiClient>();

        }
    }
}
