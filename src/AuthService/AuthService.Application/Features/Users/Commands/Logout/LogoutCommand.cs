namespace AuthService.Features.Users.Commands.Logout;

public record LogoutCommand(
     Guid UserId,
     string RefreshToken,
     string ClientId,
     bool IsLogoutFromAllDevices

) : ICommand<Unit>;
