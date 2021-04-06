using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace api.Helper
{
    public static class JwtTokenHelper
    {
        public static string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }
    }
}
