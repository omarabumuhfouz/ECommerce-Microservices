using AuthService.Application.Features.RefreshTokens.DTOs;

namespace AuthService.Application.Features.Users.Commands.Login;
public record LoginCommand(
    string Email,
    string Password,
    string ClientId

) : ICommand<TokenResponseDto>;
