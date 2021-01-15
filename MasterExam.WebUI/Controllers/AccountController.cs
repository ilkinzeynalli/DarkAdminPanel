using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using DarkAdminPanel.Core.Concrete.RequestInputModels;
using DarkAdminPanel.Core.Concrete.ResponseOutputModels;
using DarkAdminPanel.WebUI.ApiClients.Abstract;
using DarkAdminPanel.WebUI.Extensions;
using DarkAdminPanel.WebUI.Models;
using DarkAdminPanel.WebUI.Services.Abstract;
using DarkAdminPanel.WebUI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DarkAdminPanel.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService _loginManager;
        private readonly IAccountApiClient _accountApiClient;

        public AccountController(IAccountApiClient accountApiClient, ILoginService loginManager)
        {
            _accountApiClient = accountApiClient;
            _loginManager = loginManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model,  string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountApiClient.LoginAsync(model);

                string result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jwt = JsonConvert.DeserializeObject<JWT>(result);

                    _loginManager.Token = jwt.Token;

                    return Redirect(returnUrl ?? "/Home/Index");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var badRequest =  JsonConvert.DeserializeObject<BadRequest>(result);

                    foreach (var key in badRequest.Errors.Keys)
                    {
                        foreach (var value in badRequest.Errors[key])
                        {
                            ModelState.AddModelError(key, value);
                        }
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", "Email ve ya Sifre yalnisdir");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _loginManager.Logout();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
               var response = await _accountApiClient.RegisterAsync(model);
               string result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    TempData["Message"] = "User basariyla yarildi";
                    return RedirectToAction("Login", "Account");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var badRequest = JsonConvert.DeserializeObject<BadRequest>(result);

                    foreach (var key in badRequest.Errors.Keys)
                    {
                        foreach (var value in badRequest.Errors[key])
                        {
                            ModelState.AddModelError(key, value);
                        }
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    var conflict = JsonConvert.DeserializeObject<Response>(result);

                    ModelState.AddModelError("", conflict.Message);
                }
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var internalServerError = JsonConvert.DeserializeObject<Response>(result);

                    ModelState.AddModelError("", internalServerError.Message);
                }

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Setting()
        {
            var user = await _accountApiClient.GetUserByNameAsync(_loginManager.UserName);
            string result = await user.Content.ReadAsStringAsync();

            if (user.StatusCode == HttpStatusCode.OK)
            {
                var existUser = JsonConvert.DeserializeObject<AccountSettingViewModel>(result);

                return View(existUser);
            }
            else if (user.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
          
            return View();
        }
    }
}
