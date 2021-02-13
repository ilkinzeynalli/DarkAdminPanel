using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using DarkAdminPanel.DataAccess.Concrete.EntityFramework.Contexts;
using DarkAdminPanel.DataAccess.Concrete.EntityFramework.IdentityModels;
using DarkAdminPanel.WebApi.Extensions;
using DarkAdminPanel.WebApi.Models;
using DarkAdminPanel.WebApi.Models.RequestInputModels;
using DarkAdminPanel.WebApi.Models.ResponseOutputModels;
using DarkAdminPanel.WebApi.Routing;
using DarkAdminPanel.WebApi.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DarkAdminPanel.WebApi.Controllers
{
    
    [ApiController]
    [ApiRoutePrefix("token")]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public TokenController(UserManager<ApplicationUser> userManager,ITokenService tokenService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenApiInputModel model)
        {
            if (model is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = model.AccessToken;
            string refreshToken = model.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = await _userManager.FindByNameAsync(userName);

            //Exist tokens find
            var existTokens = await _context.ApplicationUserToken.Where(f => f.UserId == user.Id).ToListAsync();

            var existAccessToken = existTokens.FirstOrDefault(f => f.Name == TokenTypes.AccessToken);
            var existRefreshToken = existTokens.FirstOrDefault(f => f.Name == TokenTypes.RefreshToken);

            if (user == null || existRefreshToken.Value != refreshToken || existRefreshToken.ExpireDate <= DateTime.Now)
            {
                return Unauthorized("Refresh token expired");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();


            existAccessToken.Value = newAccessToken;
            existAccessToken.ExpireDate = new JwtSecurityToken(newAccessToken).ValidTo.ConvertUtcToLocalTime();

            existRefreshToken.Value = newRefreshToken;
            existRefreshToken.ExpireDate = DateTime.Now.AddMinutes(5);

            _context.SaveChanges();
            
            return Ok(new TokenApiOutputModel() { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return BadRequest();

            //Exist tokens find
            var existTokens = _context.ApplicationUserToken.Where(f => f.UserId == user.Id &&
                                                (f.Name == TokenTypes.AccessToken || f.Name == TokenTypes.RefreshToken));
            if (existTokens != null)
            {
                foreach (var existToken in existTokens)
                {
                    existToken.Value = null;
                    existToken.ExpireDate = new DateTime(1900,1,1);
                }
            }
          
            _context.SaveChanges();

            return Ok("Tokens revoked");
        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public async Task<IActionResult> Validate([FromQuery] string token)
        {
            var result = await Task.Run(() => _tokenService.ValidateToken(token));

            return Ok(result);
        }

        
    }
}
