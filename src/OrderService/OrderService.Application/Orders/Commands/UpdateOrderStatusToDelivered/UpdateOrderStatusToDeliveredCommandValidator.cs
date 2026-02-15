using CustomerService.Application.Extensions;
using FluentValidation;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToDelivered;

public class UpdateOrderStatusToDeliveredCommandValidator : AbstractValidator<UpdateOrderStatusToDeliveredCommand>
{
    public UpdateOrderStatusToDeliveredCommandValidator()
    {
        RuleFor(x => x.OrderId).ValidateOrderId();
        RuleFor(x => x.PaymentId).ValidatePaymentId();
    }
}