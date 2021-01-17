using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkAdminPanel.WebUI.Attributes;
using DarkAdminPanel.WebUI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkAdminPanel.WebUI.Controllers
{
    [AuthorizeRoles(Roles.User)]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
