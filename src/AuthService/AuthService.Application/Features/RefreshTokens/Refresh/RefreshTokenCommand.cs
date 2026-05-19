using AuthService.Application.Features.RefreshTokens.DTOs;

namespace AuthService.Application.Features.RefreshTokens.Refresh;

public record RefreshTokenCommand(
 string RefreshToken,
 string ClientId
)
 : ICommand<TokenResponseDto>;