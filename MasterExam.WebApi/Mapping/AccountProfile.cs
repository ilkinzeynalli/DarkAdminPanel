using AutoMapper;
using DarkAdminPanel.DataAccess.Concrete.EntityFramework.IndentityModels;
using DarkAdminPanel.WebApi.ResponseOutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebApi.Mapping
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<ApplicationUser, UserByNameGetOutputModel>()
                .ForMember(dest=>dest.UserId,opt=>opt.MapFrom(m=>m.Id));
        }
    }
}
