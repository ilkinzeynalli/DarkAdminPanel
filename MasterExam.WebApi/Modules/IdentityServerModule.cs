using DarkAdminPanel.DataAccess.Concrete.EntityFramework.Contexts;
using DarkAdminPanel.DataAccess.Concrete.EntityFramework.IndentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.Modules
{
    public static class IdentityServerModule
    {
        public static void Load(IServiceCollection services)
        {
            //Adding Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                        //Identity settings
                        options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
