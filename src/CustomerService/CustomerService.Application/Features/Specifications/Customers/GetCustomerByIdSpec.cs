using Ardalis.Specification;
using CustomerService.Domain.Customers;

namespace CustomerService.Application.Features.Specifications.Customers;

public class GetCustomerByIdSpec : Specification<Customer>
{
    public GetCustomerByIdSpec(Guid customerId, bool withTracking = false)
    {
        if (withTracking) Query.AsTracking();

        Query.Include(c => c.Addresses);

        Query.Where(c => c.Id == customerId);
    }
}