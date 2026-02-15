using AuthService.Application.Features.RefreshTokens.DTOs;
using SharedKernel.Abstractions.Messaging;

namespace AuthService.Application.Features.RefreshTokens.Refresh;

public record RefreshTokenCommand(
 string RefreshToken,
 string ClientId
)
 : ICommand<TokenResponseDto>;