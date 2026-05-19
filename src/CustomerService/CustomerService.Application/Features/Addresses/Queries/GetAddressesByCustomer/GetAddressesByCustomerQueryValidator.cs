namespace CustomerService.Application.Features.Addresses.Queries.GetAddressesByCustomer;

public class GetAddressesByCustomerQueryValidator : AbstractValidator<GetAddressesByCustomerQuery>
{
    public GetAddressesByCustomerQueryValidator()
    {
        RuleFor(d => d.CustomerId).ValidateCustomerId();

        
    }
}