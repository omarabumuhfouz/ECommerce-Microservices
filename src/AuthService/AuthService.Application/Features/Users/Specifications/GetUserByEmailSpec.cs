namespace AuthService.Application.Features.Users.Specifications;

public class GetUserByEmailSpec : Specification<User>
{
    public GetUserByEmailSpec(string email, bool withTracking = false) : base(u => u.Email == email)
    {
        if (withTracking) EnableTracking();
        
    }
}