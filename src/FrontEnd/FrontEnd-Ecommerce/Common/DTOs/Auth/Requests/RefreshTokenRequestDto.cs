namespace FrontEnd_Ecommerce.DTOs.Auth.Requests;

public record RefreshTokenRequestDto(string RefreshToken, string ClientId);