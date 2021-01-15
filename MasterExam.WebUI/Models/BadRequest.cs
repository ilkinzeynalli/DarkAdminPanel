using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Models
{
    public class BadRequest
    {
        public int Status { get; set; }
        public Dictionary<string,List<string>> Errors { get; set; }
    }
}
