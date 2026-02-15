using FrontEnd_Ecommerce.DTOs.Auth.Requests;

namespace FrontEnd_Ecommerce.Services.Interfaces;


public interface ITokenService
{
    Task<bool> RefreshTokenAsync();
}