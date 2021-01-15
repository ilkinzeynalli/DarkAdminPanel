using DarkAdminPanel.Core.Concrete.RequestInputModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.FluentValidations
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(p => p.Email).NotNull().WithMessage("Email daxil edin");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Email daxil edin");

            RuleFor(p => p.Password).NotNull().WithMessage("Password daxil edin");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password daxil edin");

        }
    }
}
