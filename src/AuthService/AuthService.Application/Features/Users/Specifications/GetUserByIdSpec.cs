using Ardalis.Specification;

namespace AuthService.Application.Features.Users.Specifications;

public class GetUserByIdSpec : Specification<User>
{
    public GetUserByIdSpec(Guid userId, bool withTracking = false)
    {
        Query.Where(u => u.Id == userId);
        if (withTracking) Query.AsTracking();
    
    }

}
