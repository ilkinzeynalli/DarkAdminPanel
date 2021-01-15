using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkAdminPanel.Core.Concrete.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarkAdminPanel.WebUI.Controllers
{
    [Authorize(Roles = Roles.User)]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
