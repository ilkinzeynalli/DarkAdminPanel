using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DarkAdminPanel.Core.Concrete.Models;
using DarkAdminPanel.DataAccess.Concrete.EntityFramework.Contexts;
using DarkAdminPanel.Entities.Concrete;
using DarkAdminPanel.WebApi.Attributes;
using DarkAdminPanel.WebApi.FluentValidations;
using DarkAdminPanel.WebApi.RequestInputModels;
using DarkAdminPanel.WebApi.ResponseOutputModels;
using DarkAdminPanel.WebApi.Routing;
using DarkAdminPanel.WebApi.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DarkAdminPanel.WebApi.Controllers
{
    [AuthorizeRoles(Roles.Admin, Roles.User)]
    [ApiRoutePrefix("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public AccountController(UserManager<ApplicationUser> userManager,
                                RoleManager<ApplicationRole> roleManager,
                                IConfiguration configuration,
                                IMapper mapper,
                                ITokenService tokenService,
                                ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                    foreach (var userRole in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                  
                    var accessToken = _tokenService.GenerateAccessToken(claims);
                    var refreshToken = _tokenService.GenerateRefreshToken();

                    //refresh token add degisecem burayi kecici etmisem
                    _context.ApplicationUserToken.Add(new ApplicationUserToken()
                    {
                        User = user,
                        LoginProvider = "MyApp",
                        Name = "RefreshToken",
                        Value = refreshToken,
                        ExpireDate = DateTime.Now.AddMinutes(5)
                    });

                    _context.SaveChanges();

                    await _userManager.UpdateAsync(user);

                    return Ok(new
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken
                    });
                }

                return Unauthorized();
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);

                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseOutputModel { Status = StatusCodes.Status500InternalServerError, Message = "User already exists!" });

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseOutputModel { Status = StatusCodes.Status500InternalServerError, Message = "User creation failed! Please check user details and try again." });

                if (!await _roleManager.RoleExistsAsync(Roles.User))
                    await _roleManager.CreateAsync(new ApplicationRole { Name = Roles.User });

                if (await _roleManager.RoleExistsAsync(Roles.User))
                    await _userManager.AddToRoleAsync(user, Roles.User);

                return Ok();
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByNameAsync(model.UserName);

                if (userExists != null)
                    return StatusCode(StatusCodes.Status409Conflict, new ResponseOutputModel { Status = StatusCodes.Status409Conflict, Message = "User already exists!" });

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseOutputModel { Status = StatusCodes.Status500InternalServerError, Message = "User creation failed! Please check user details and try again." });

                if (!await _roleManager.RoleExistsAsync(Roles.Admin))
                    await _roleManager.CreateAsync(new ApplicationRole() { Name = Roles.Admin });

                if (await _roleManager.RoleExistsAsync(Roles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("getUserByName/{userName}")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var result = _mapper.Map<UserByNameGetOutputModel>(user);
                return Ok(result);
            }

            return NotFound();
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (token != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (result.Succeeded)
                    return Ok();
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "Password was not reset successfully");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Couldn't assign token");
        }
    }
}
