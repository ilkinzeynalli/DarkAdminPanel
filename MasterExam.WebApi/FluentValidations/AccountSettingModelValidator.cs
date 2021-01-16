using DarkAdminPanel.Core.Concrete.RequestInputModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.FluentValidations
{
    public class AccountSettingModelValidator: AbstractValidator<AccountSettingModel>
    {
        public AccountSettingModelValidator()
        {
            RuleFor(r => r.Password).NotEmpty().WithMessage("Password daxil edin");

            RuleFor(r => r.ConfirmPassword).NotEmpty().WithMessage("ConfirmPassword daxil edin");

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Password != x.ConfirmPassword)
                {
                    context.AddFailure(nameof(x.Password), "Passwordlar uygun gelmir");
                }
            });
        }
    }
}
