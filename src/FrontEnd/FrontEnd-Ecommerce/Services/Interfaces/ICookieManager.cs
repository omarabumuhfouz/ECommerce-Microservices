namespace FrontEnd_Ecommerce.Services.Interfaces;

public interface ICookieManager
{
    void Set(string key, string value, int expireMinutes);
    string? Get(string key);
    void Remove(string key);
    string? GetAccessToken();
    string GetRefreshToken();
    void SetRefreshToken(string refreshToken);
    void SetAccessToken(string token);
    void ClearAllCookies();          
    void ClearAuthCookies();   
    void ClearTokens();               
}