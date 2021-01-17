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
            //var token = HttpContext.Session.GetString("JWToken");
            //var token2 = new JwtSecurityTokenHandler().ReadJwtToken(token);
            //var claim = token2.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            return View();
        }
    }
}
