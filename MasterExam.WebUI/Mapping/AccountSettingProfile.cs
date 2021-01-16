using AutoMapper;
using DarkAdminPanel.Core.Concrete.RequestInputModels;
using DarkAdminPanel.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Mapping
{
    public class AccountSettingProfile :Profile
    {
        public AccountSettingProfile()
        {
            CreateMap<AccountSettingViewModel, AccountSettingModel>()
                .ForMember(dest=> dest.UserId,opt=> opt.MapFrom(map=>map.Id));
        }
    }
}
