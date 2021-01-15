using DarkAdminPanel.Core.Concrete.RequestInputModels;
using DarkAdminPanel.WebApi.FluentValidations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.Modules
{
    public static class ValidatorModule
    {
        public static void Load(IServiceCollection services)
        {
            services.AddTransient<IValidator<LoginModel>, LoginModelValidator>();
            services.AddTransient<IValidator<RegisterModel>, RegisterModelValidator>();
        }
    }
}
