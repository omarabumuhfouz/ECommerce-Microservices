using FluentValidation;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;

public class UpdateOrderStatusToCanceledCommandValidator : AbstractValidator<UpdateOrderStatusToCanceledCommand>
{
    public UpdateOrderStatusToCanceledCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}