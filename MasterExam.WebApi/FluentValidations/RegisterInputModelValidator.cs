using DarkAdminPanel.WebApi.RequestInputModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.FluentValidations
{
    public class RegisterInputModelValidator:AbstractValidator<RegisterInputModel>
    {
        public RegisterInputModelValidator()
        {
            RuleFor(p => p.UserName).NotNull().WithMessage("User Name daxil edin");
            RuleFor(p => p.UserName).NotEmpty().WithMessage("User Name daxil edin");
            RuleFor(p => p.UserName).EmailAddress().WithMessage("Kecerli User Name daxil edin");

            RuleFor(p => p.Password).NotNull().WithMessage("Password daxil edin");
            RuleFor(p => p.Password).NotEmpty().WithMessage("Password daxil edin");

            RuleFor(p => p.Email).NotNull().WithMessage("Emaili daxil edin");
            RuleFor(p => p.Email).NotEmpty().WithMessage("Emaili daxil edin");
            RuleFor(p => p.Email).EmailAddress().WithMessage("Kecerli email daxil edin");


        }
    }
}
