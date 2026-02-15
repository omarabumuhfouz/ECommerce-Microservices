using CustomerService.Application.Customers.Queries;
using CustomerService.Application.Extensions;

namespace CustomerService.Application.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(x => x.CustomerId).ValidateCustomerId();
    }
}
