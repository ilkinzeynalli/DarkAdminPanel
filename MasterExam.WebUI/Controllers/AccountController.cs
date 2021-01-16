using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
        public readonly IMapper _mapper;

        public AccountController(IAccountApiClient accountApiClient, ILoginService loginManager, IMapper mapper)
        {
            _accountApiClient = accountApiClient;
            _loginManager = loginManager;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountApiClient.LoginAsync(model);

                string result = await response.Content.ReadAsStringAsync();

                switch ((int)response.StatusCode)
                {
                    case (int)HttpStatusCode.OK:
                        var jwt = JsonConvert.DeserializeObject<JWT>(result);
                        _loginManager.Token = jwt.Token;

                        return Redirect(returnUrl ?? "/Home/Index");

                    case (int)HttpStatusCode.BadRequest:
                        var badRequest = JsonConvert.DeserializeObject<BadRequest>(result);
                        foreach (var key in badRequest.Errors.Keys)
                            foreach (var value in badRequest.Errors[key])
                                ModelState.AddModelError(key, value);
                        break;

                    case (int)HttpStatusCode.Conflict:
                    case (int)HttpStatusCode.InternalServerError:
                        var errors = JsonConvert.DeserializeObject<Response>(result);
                        ModelState.AddModelError("", errors.Message);
                        break;

                    case (int)HttpStatusCode.Unauthorized:
                        ModelState.AddModelError("", "Email ve ya Sifre yalnisdir");
                        break;
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

                switch ((int)response.StatusCode)
                {
                    case (int)HttpStatusCode.OK:
                        TempData["Message"] = "User basariyla yarildi";
                        return RedirectToAction("Login", "Account");

                    case (int)HttpStatusCode.BadRequest:
                        var badRequest = JsonConvert.DeserializeObject<BadRequest>(result);
                        foreach (var key in badRequest.Errors.Keys)
                            foreach (var value in badRequest.Errors[key])
                                ModelState.AddModelError(key, value);
                        break;

                    case (int)HttpStatusCode.Conflict:
                    case (int)HttpStatusCode.InternalServerError:
                        var errors = JsonConvert.DeserializeObject<Response>(result);
                        ModelState.AddModelError("", errors.Message);
                        break;

                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Setting()
        {
            var response = await _accountApiClient.GetUserByNameAsync(_loginManager.UserName);

            string result = await response.Content.ReadAsStringAsync();

            switch ((int)response.StatusCode)
            {
                case (int)HttpStatusCode.OK:
                    var existUser = JsonConvert.DeserializeObject<AccountSettingViewModel>(result);
                    return View(existUser);
                case (int)HttpStatusCode.NotFound:
                    return NotFound();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Setting(AccountSettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accountSettingModel = _mapper.Map<AccountSettingModel>(model);

                var response = await _accountApiClient.ChangePasswordAsync(accountSettingModel);
                string result = await response.Content.ReadAsStringAsync();

                switch ((int)response.StatusCode)
                {
                    case (int)HttpStatusCode.OK:
                        _loginManager.Logout();
                        return RedirectToAction("Login", "Account");

                    case (int)HttpStatusCode.BadRequest:
                        var badRequest = JsonConvert.DeserializeObject<BadRequest>(result);
                        foreach (var key in badRequest.Errors.Keys)
                            foreach (var value in badRequest.Errors[key])
                                ModelState.AddModelError(key, value);
                        break;

                    case (int)HttpStatusCode.Conflict:
                    case (int)HttpStatusCode.InternalServerError:
                        var errors = JsonConvert.DeserializeObject<Response>(result);
                        ModelState.AddModelError("", errors.Message);
                        break;

                }
            }

            return View(model);
        }
    }
}
