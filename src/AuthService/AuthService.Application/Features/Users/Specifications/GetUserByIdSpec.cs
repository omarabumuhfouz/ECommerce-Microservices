namespace AuthService.Application.Features.Users.Specifications;

public class GetUserByIdSpec : Specification<User>
{
    public GetUserByIdSpec(Guid userId, bool withTracking = false): base(u => u.Id == userId)
    {
        if (withTracking) EnableTracking();
    
    }

}
