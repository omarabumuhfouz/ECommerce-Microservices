namespace FrontEnd_Ecommerce.DTOs.Auth.Requests;

public record LogoutRequestDto(
     string RefreshToken,
     string ClientId,
     bool IsLogoutFromAllDevices
);