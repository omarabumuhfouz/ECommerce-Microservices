using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FrontEnd_Ecommerce.Exceptions;

namespace FrontEnd_Ecommerce.Services.Helpers;

public class JwtHelper(ICookieManager cookiemanager)
{
    public Dictionary<string, string> GetClaimsFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token); // decode token
        var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
        return claims;
    }


    public Guid GetUserId()
    {
        var sub = GetClaimsFromToken(cookiemanager.GetAccessToken())[ClaimTypes.NameIdentifier];
        if (sub is null)
            throw new UnauthorizedException("Sub is null.");

        Guid.TryParse(sub, out Guid userId);
        return userId;

    }

    public string GetUserName()
    {
        var name = GetClaimsFromToken(cookiemanager.GetAccessToken())[ClaimTypes.Name];
        return name ?? "Unknown User";
    }

    public string GetRole()
    {
        return GetClaimsFromToken(cookiemanager.GetAccessToken())[ClaimTypes.Role];
    }



}
