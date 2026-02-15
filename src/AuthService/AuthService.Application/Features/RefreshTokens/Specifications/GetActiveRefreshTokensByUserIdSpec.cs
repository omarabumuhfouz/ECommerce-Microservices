namespace AuthService.Application.Features.RefreshTokens.Specifications;

public sealed class GetActiveRefreshTokensByUserIdSpec : Specification<RefreshToken>
{
    public GetActiveRefreshTokensByUserIdSpec(Guid userId)
            : base(rt => rt.UserId == userId && !rt.IsRevoked)
    {
    }
}