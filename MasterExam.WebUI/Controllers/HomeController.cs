using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DarkAdminPanel.WebUI.Attributes;
using DarkAdminPanel.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DarkAdminPanel.WebUI.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizeRoles(Roles.Admin,Roles.User)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
