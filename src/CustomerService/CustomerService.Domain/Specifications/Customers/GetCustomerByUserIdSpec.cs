using System.ComponentModel;
using System.Linq.Expressions;
using CustomerService.Domain.Entities;
using SharedKernel.Specifications;

namespace CustomerService.Domain.Specifications.Customers;

public class GetCustomerByUserIdSpec : Specification<Customer>
{
    public GetCustomerByUserIdSpec(Guid userId) : base(c => c.UserId == userId)
    {
        AddInclude(c => c.Addresses);
    }
}