using CustomerService.Application.Extensions;

namespace OrderService.Application.Orders.Queries.GetOrdersByCustomer;
public class GetOrdersByCustomerQueryValidator : AbstractValidator<GetOrdersByCustomerQuery>
{
    public GetOrdersByCustomerQueryValidator()
    {
        RuleFor(c => c.CustomerId)
            .ValidateCustomerId();
    }
}