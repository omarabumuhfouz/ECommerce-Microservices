using Ardalis.Specification;

namespace AuthService.Application.Features.Users.Specifications;

public class GetUserByEmailSpec : Specification<User>
{
    public GetUserByEmailSpec(string email, bool withTracking = false)
    {
        Query.Where(u => u.Email.ToLower() == email.ToLower()); 
        if (withTracking) Query.AsTracking(); 
        
    }
}