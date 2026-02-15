namespace AuthService.Application.Features.RefreshTokens.Specifications;

public sealed class GetRefreshTokenByTokenAndClientSpec : Specification<RefreshToken>
{
    public GetRefreshTokenByTokenAndClientSpec(string hashedToken, Guid clientId)
           : base(rt => rt.Token == hashedToken 
                  &&
                  rt.ClientId == clientId)
    {
    }
}