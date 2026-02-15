using System.ComponentModel;
using CustomerService.Domain.Entities;
using SharedKernel.Specifications;

namespace CustomerService.Domain.Specifications.Customers;

public class GetCustomerByIdSpec : Specification<Customer>
{
    public GetCustomerByIdSpec(Guid customerId, bool withTracking = false) : base(c => c.Id == customerId)
    {
        AddInclude(c => c.Addresses);

        if (withTracking) EnableTracking();
    }
}