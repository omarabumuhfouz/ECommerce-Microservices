using CustomerService.Application.Extensions;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToRefunded;

public class UpdateOrderStatusToRefundedCommandValidator : AbstractValidator<UpdateOrderStatusToRefundedCommand>
{
    public UpdateOrderStatusToRefundedCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .ValidateOrderId();
    }
}
