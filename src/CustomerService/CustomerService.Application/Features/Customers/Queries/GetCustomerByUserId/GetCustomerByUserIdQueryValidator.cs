using CustomerService.Application.Customers.Queries;
using CustomerService.Application.Extensions;

namespace CustomerService.Application.Customers.Queries.GetCustomerByUserId;

public class GetCustomerByUserIdQueryValidator : AbstractValidator<GetCustomerByUserIdQuery>
{
    public GetCustomerByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId).ValidateUserId();
    }
}
