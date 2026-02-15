using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SharedKernel.Services;
public class JwksKeyProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly string _jwksUrl;

    public JwksKeyProvider(IHttpClientFactory httpClientFactory, IMemoryCache cache, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _jwksUrl = config["Jwt:JWKS"]!;
    }

    public IEnumerable<SecurityKey> GetKeys(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
    {
        return _cache.GetOrCreate("JWKS_KEYS", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            var client = _httpClientFactory.CreateClient();
            var response = client.GetStringAsync(_jwksUrl).Result; 
            return new JsonWebKeySet(response).Keys.Cast<SecurityKey>();
        })!;
    }
}