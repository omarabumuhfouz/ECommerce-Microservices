using System.Net.Http.Json;
using FrontEnd_Ecommerce.DTOs.Auth.Requests;
using FrontEnd_Ecommerce.DTOs.Auth.Responses;
using FrontEnd_Ecommerce.Services.Helpers;

namespace FrontEnd_Ecommerce.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly ICookieManager _cookieManager;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IHttpClientFactory httpClientFactory, ICookieManager cookieManager, ILogger<TokenService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ApiGateway");
        
        _cookieManager = cookieManager;
        _logger = logger;
    }

    public async Task<bool> RefreshTokenAsync()
    {
        try
        {
            var currentRefresh = _cookieManager.GetRefreshToken();
            
            if (string.IsNullOrEmpty(currentRefresh)) return false;

            var requestDto = new RefreshTokenRequestDto(currentRefresh, "myapp");

            var response = await _httpClient.PostAsJsonAsync(Endpoints.Auth.RefreshToken, requestDto);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Refresh failed with status: {StatusCode}", response.StatusCode);
                return false;
            }

            var newTokens = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

            if (newTokens is null) return false;

            _cookieManager.SetAccessToken(newTokens.Token);
            _cookieManager.SetRefreshToken(newTokens.RefreshToken);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during token refresh.");
            return false;
        }
    }
}