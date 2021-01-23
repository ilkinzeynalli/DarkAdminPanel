using DarkAdminPanel.WebApi.Services.Abstract;
using DarkAdminPanel.WebApi.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.Modules
{
    public static class LogicModule
    {
        public static void Load(IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenManager>();
        }
    }
}
