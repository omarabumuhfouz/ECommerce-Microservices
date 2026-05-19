using Ardalis.Specification;

namespace AuthService.Application.Features.RefreshTokens.Specifications;

public sealed class GetActiveRefreshTokensByUserIdSpec : Specification<RefreshToken>
{
    public GetActiveRefreshTokensByUserIdSpec(Guid userId)
    {
        Query.Where(rt => rt.UserId == userId && !rt.IsRevoked);
    }
}