using Ardalis.Specification;

namespace AuthService.Application.Features.RefreshTokens.Specifications;

public sealed class GetRefreshTokenForRotationSpec : Specification<RefreshToken>
{
    public GetRefreshTokenForRotationSpec(string hashedToken, Guid userId, Guid dbClientId, bool withTracking = false)
    {
        Query.Where(rt => rt.Token == hashedToken &&
               //    rt.UserId == userId && 
               rt.ClientId == dbClientId);

        if (withTracking) Query.AsTracking();
    }
}