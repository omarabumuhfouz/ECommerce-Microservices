using CustomerService.Application.Extensions;
using OrderService.Domain.Orders.Enums;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .ValidateOrderId();

        RuleFor(x => x.OrderStatus)
            .IsInEnum().WithMessage("The provided Order Status is not valid.");
    }
}