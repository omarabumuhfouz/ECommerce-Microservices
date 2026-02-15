using FrontEnd_Ecommerce.DTOs.Auth.Responses;

namespace FrontEnd_Ecommerce.DTOs.Auth;

public record RegisterResponseDto(ProfileDto ProfileInfo, TokenResponseDto TokenInfo);
