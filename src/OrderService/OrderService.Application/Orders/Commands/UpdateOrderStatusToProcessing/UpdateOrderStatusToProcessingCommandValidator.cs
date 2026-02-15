using CustomerService.Application.Extensions;
using FluentValidation;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToProcessing;

public class UpdateOrderStatusToProcessingCommandValidator : AbstractValidator<UpdateOrderStatusToProcessingCommand>
{
    public UpdateOrderStatusToProcessingCommandValidator()
    {
        RuleFor(x => x.OrderId).ValidateOrderId();

        RuleFor(x => x.PaymentId).ValidatePaymentId();
    }
}