namespace CustomerService.Application.Features.Customers.Queries.GetCustomerByUserId;

public class GetCustomerByUserIdQueryValidator : AbstractValidator<GetCustomerByUserIdQuery>
{
    public GetCustomerByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId).ValidateUserId();
    }
}
