using FrontEnd_Ecommerce.DTOs.Auth;
using FrontEnd_Ecommerce.DTOs.Auth.Requests;
using FrontEnd_Ecommerce.DTOs.Auth.Responses;

namespace FrontEnd_Ecommerce.Services.Interfaces;

public interface IAuthService
{
    Task ChangePasswordAsync(ChangePasswordRequest request);
    Task<TokenResponseDto?> LoginAsync(LoginRequest request);
    Task LogoutAsync(bool IsLogoutFromAllDevices);
    Task<RegisterResponseDto?> RegisterAsync(RegisterRequest request);
}
