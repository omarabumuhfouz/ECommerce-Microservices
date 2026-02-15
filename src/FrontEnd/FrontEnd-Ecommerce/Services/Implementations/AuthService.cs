using FrontEnd_Ecommerce.DTOs.Auth;
using FrontEnd_Ecommerce.DTOs.Auth.Requests;
using FrontEnd_Ecommerce.DTOs.Auth.Responses;

namespace FrontEnd_Ecommerce.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApiRequestHandler _requestHandler;
    private readonly ICookieManager _cookieManager;

    async Task IAuthService.ChangePasswordAsync(ChangePasswordRequestDto request)
    {
        var settings = new ApiRequestSettings()
              .ToUrl(Endpoints.Auth.ChangePassword)
              .UsingMethod(HttpMethod.Post)
              .WithBody(request);

        await _requestHandler.SendRequestAsync(settings);
    }

    async Task<TokenResponseDto?> IAuthService.LoginAsync(LoginRequestDto request)
    {
        var settings = new ApiRequestSettings()
                    .ToUrl(Endpoints.Auth.Login)
                    .UsingMethod(HttpMethod.Post)
                    .WithBody(request);

        return await _requestHandler.SendRequestAsync<TokenResponseDto>(settings);
    }

    async Task IAuthService.LogoutAsync(bool IsLogoutFromAllDevices)
    {
        var refreshToken = _cookieManager.GetRefreshToken();
        var clientId = "myapp";
        var request = new LogoutRequestDto(refreshToken, clientId, IsLogoutFromAllDevices);

        var settings = new ApiRequestSettings()
                                .ToUrl(Endpoints.Auth.Logout)
                                .UsingMethod(HttpMethod.Post)
                                .WithBody(request);

        await _requestHandler.SendRequestAsync(settings);
    }

    async Task<RegisterResponseDto?> IAuthService.RegisterAsync(RegisterRequestDto request)
    {
        var settings = new ApiRequestSettings()
                    .ToUrl(Endpoints.Auth.Register)
                    .UsingMethod(HttpMethod.Post)
                    .WithBody(request);
        return await _requestHandler.SendRequestAsync<RegisterResponseDto>(settings);
    }
}
