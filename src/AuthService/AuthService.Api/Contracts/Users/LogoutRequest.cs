namespace AuthService.Api.Contracts.Users;

public record LogoutRequest(string RefreshToken, string ClientId, bool IsLogoutFromAllDevices);
