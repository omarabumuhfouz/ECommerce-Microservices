using FluentValidation;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToShipped;

public class UpdateOrderStatusToShippedCommandValidator : AbstractValidator<UpdateOrderStatusToShippedCommand>
{
    public UpdateOrderStatusToShippedCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}