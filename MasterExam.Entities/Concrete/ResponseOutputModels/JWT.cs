using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.Core.Concrete.ResponseOutputModels
{
    public class JWT
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
