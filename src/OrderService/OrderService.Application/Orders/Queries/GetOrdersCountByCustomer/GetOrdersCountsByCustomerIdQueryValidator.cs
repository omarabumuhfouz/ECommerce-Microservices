using CustomerService.Application.Extensions;

namespace OrderService.Application.Orders.Queries.GetOrdersCountByCustomer;
public class GetOrdersCountsByCustomerIdQueryValidator : AbstractValidator<GetOrdersCountsByCustomerIdQuery>
{
    public GetOrdersCountsByCustomerIdQueryValidator()
    {
        RuleFor(c => c.CustomerId)
            .ValidateCustomerId();
    }
}