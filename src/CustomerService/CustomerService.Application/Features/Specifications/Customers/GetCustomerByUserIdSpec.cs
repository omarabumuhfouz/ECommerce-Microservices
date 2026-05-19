using Ardalis.Specification;
using CustomerService.Domain.Customers;

namespace CustomerService.Domain.Specifications.Customers;

public class GetCustomerByUserIdSpec : Specification<Customer>
{
    public GetCustomerByUserIdSpec(Guid userId)
    {
        Query.Include(c => c.Addresses);

        Query.Where(c => c.UserId == userId);
    }
}