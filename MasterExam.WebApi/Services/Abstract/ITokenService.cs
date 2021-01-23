using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace DarkAdminPanel.WebApi.Services.Abstract
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool ValidateToken(string token);
    }
}
