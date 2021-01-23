using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DarkAdminPanel.Entities.Concrete
{
    public class ApplicationUserToken :IdentityUserToken<string>
    {
        public DateTime ExpireDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
