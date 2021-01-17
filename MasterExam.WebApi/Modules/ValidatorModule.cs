using DarkAdminPanel.WebApi.FluentValidations;
using DarkAdminPanel.WebApi.RequestInputModels;
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
            services.AddTransient<IValidator<LoginInputModel>, LoginInputModelValidator>();
            services.AddTransient<IValidator<RegisterInputModel>, RegisterInputModelValidator>();
            services.AddTransient<IValidator<ChangePasswordInputModel>, AccountSettingInputModelValidator>();
        }
    }
}
