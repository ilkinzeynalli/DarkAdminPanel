using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkAdminPanel.Core.Concrete.Attributes;
using DarkAdminPanel.Core.Concrete.Models;
using DarkAdminPanel.WebApi.Routing;
//using DarkAdminPanel.Core.Concrete.Attributes;
//using DarkAdminPanel.Core.Concrete.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DarkAdminPanel.WebApi.Controllers
{
    [ApiRoutePrefix("account")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [AuthorizeRoles(Roles.User, Roles.Admin)]
        [HttpGet("index")]
        public IActionResult Index()
        {
            string[] names = { "ilkin", "zemfira" };

            return Ok(names);
        }
    }
}
