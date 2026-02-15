using CustomerService.Application.Extensions;
using OrderService.Application.Orders.Queries.GetOrderById;

namespace OrderService.Application.Orders.Validators;

public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .ValidateOrderId();
    }
}