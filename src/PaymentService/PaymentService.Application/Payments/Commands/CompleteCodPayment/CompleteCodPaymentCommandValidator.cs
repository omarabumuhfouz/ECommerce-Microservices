using PaymentService.Application.Common.Extensions;
using PaymentService.Domain.Errors; // Import your extensions

namespace PaymentService.Application.Payments.Commands.CompleteCodPayment;

public class CompleteCodPaymentCommandValidator : AbstractValidator<CompleteCodPaymentCommand>
{
    public CompleteCodPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .ValidatePaymentId();

        RuleFor(x => x.OrderId)
            .ValidateOrderId();
    }
}
