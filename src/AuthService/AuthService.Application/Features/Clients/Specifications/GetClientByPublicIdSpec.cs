using Ardalis.Specification;
namespace AuthService.Application.Features.Clients.Specifications;

public class GetClientByPublicIdSpec : Specification<Client>
{
    public GetClientByPublicIdSpec(string clientId, bool withTracking = false)
    {
        Query.Where(c => c.ClientId == clientId);

        if (withTracking) Query.AsNoTracking();
        
    }
}