using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DarkAdminPanel.Core.Concrete.Attributes;
using DarkAdminPanel.Core.Concrete.Models;
using DarkAdminPanel.Core.Concrete.RequestInputModels;
using DarkAdminPanel.Core.Concrete.ResponseOutputModels;
using DarkAdminPanel.DataAccess.Concrete.EntityFramework.IndentityModels;
using DarkAdminPanel.WebApi.FluentValidations;
using DarkAdminPanel.WebApi.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DarkAdminPanel.WebApi.Controllers
{
    [ApiRoutePrefix("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager,
                                RoleManager<ApplicationRole> roleManager,
                                IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    return Ok(new JWT { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo });
                }

                return Forbid();
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);

                if (userExists != null)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = StatusCodes.Status500InternalServerError, Message = "User already exists!" });

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = StatusCodes.Status500InternalServerError, Message = "User creation failed! Please check user details and try again." });

                if (!await _roleManager.RoleExistsAsync(Roles.User))
                    await _roleManager.CreateAsync(new ApplicationRole { Name = Roles.User });

                if (await _roleManager.RoleExistsAsync(Roles.User))
                    await _userManager.AddToRoleAsync(user, Roles.User);

                return Ok();
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByNameAsync(model.UserName);

                if (userExists != null)
                    return StatusCode(StatusCodes.Status409Conflict, new Response { Status = StatusCodes.Status409Conflict, Message = "User already exists!" });

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = StatusCodes.Status500InternalServerError, Message = "User creation failed! Please check user details and try again." });

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

        [AuthorizeRoles(Roles.Admin,Roles.User)]
        [HttpGet("[action]/{userName}")]
        public async Task<IActionResult> GetUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
                return Ok(user);

            return NotFound();

        }

    }
}
