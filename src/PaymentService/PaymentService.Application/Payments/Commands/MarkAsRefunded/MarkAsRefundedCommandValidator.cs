using PaymentService.Application.Common.Extensions;

namespace PaymentService.Application.Payments.Commands.MarkAsRefunded;

public sealed class MarkAsRefundedCommandValidator : AbstractValidator<MarkAsRefundedCommand>
{
    public MarkAsRefundedCommandValidator()
    {
        RuleFor(x => x.OrderId).ValidateOrderId();
    }
}
