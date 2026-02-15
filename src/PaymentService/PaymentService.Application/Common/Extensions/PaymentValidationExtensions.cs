using PaymentService.Domain.Errors;

namespace PaymentService.Application.Common.Extensions;

public static class PaymentValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> ValidatePaymentId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Payment ID is required.")
            .WithErrorCode(ErrorCodes.Payment.PaymentIdRequired);
    }

    public static IRuleBuilderOptions<T, Guid> ValidateOrderId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Order ID is required.")
            .WithErrorCode(ErrorCodes.Payment.OrderIdRequired);
    }
}