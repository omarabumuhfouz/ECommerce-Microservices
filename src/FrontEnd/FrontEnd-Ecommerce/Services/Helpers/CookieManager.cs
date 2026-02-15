using FrontEnd_Ecommerce.Exceptions;

namespace FrontEnd_Ecommerce.Services.Helpers;

public class CookieManager : ICookieManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Set(string key, string value, int expireMinutes)
    {
        var options = new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(expireMinutes), HttpOnly = true };
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
    }


    public string? Get(string key)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            return null;

        context.Request.Cookies.TryGetValue(key, out var value);
        return value;
    }

    

    public void Remove(string key)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
    }

    public string GetAccessToken()
    {
        return Get(CookieKeys.AccessToken)
                    ?? throw new MissingTokenException();
    }

    public string GetRefreshToken()
    {
        return Get(CookieKeys.RefreshToken)
                    ?? throw new MissingTokenException();
    }

    public void SetRefreshToken(string refreshToke)
    {
        Set(CookieKeys.RefreshToken, refreshToke , CookieSettings.ExpireMinutes );
    }

    public void SetAccessToken(string token)
    {
        Set(CookieKeys.AccessToken, token, CookieSettings.ExpireMinutes);
    }

    public void ClearAllCookies()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        // Get all cookie keys from the request
        var cookieKeys = httpContext.Request.Cookies.Keys;

        // Delete each cookie
        foreach (var key in cookieKeys)
        {
            httpContext.Response.Cookies.Delete(key);
        }
    }

    public void ClearAuthCookies()
    {
        Remove(CookieKeys.AccessToken);
        Remove(CookieKeys.RefreshToken);
        
        // Remove any other auth-related cookies you might have
        // Remove("UserSession");
        // Remove("UserPreferences");
    }

    public void ClearTokens()
    {
        Remove(CookieKeys.AccessToken);
        Remove(CookieKeys.RefreshToken);
    }
}