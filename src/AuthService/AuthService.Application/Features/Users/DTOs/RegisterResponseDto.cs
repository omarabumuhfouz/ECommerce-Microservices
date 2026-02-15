using AuthService.Application.Features.RefreshTokens.DTOs;

namespace AuthService.Application.Features.Users.DTOs;

public record RegisterResponseDto(ProfileDto ProfileInfo, TokenResponseDto TokenInfo);
