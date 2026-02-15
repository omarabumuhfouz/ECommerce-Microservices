namespace AuthService.Application.Features.Clients.Specifications;

public class GetClientByPublicIdSpec : Specification<Client>
{
    public GetClientByPublicIdSpec(string clinetId, bool withTracking = false) : base(c => c.ClientId == clinetId)
    {
        if (withTracking) EnableTracking();
        
    }
}