using CustomerService.Application.Addresses.Queries;

namespace CustomerService.Application.Addresses.Queries.GetAddressesByCustomer;

public class GetAddressesByCustomerQueryValidator : AbstractValidator<GetAddressesByCustomerQuery>
{
    public GetAddressesByCustomerQueryValidator()
    {
        RuleFor(d => d.CustomerId).ValidateCustomerId();

        
    }
}