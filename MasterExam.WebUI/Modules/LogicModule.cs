using DarkAdminPanel.WebUI.Services.Abstract;
using DarkAdminPanel.WebUI.Services.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Modules
{
    public static class LogicModule
    {
        public static void Load(IServiceCollection services)
        {
            services.AddScoped<ILoginService, LoginManager>();
        }
    }
}
