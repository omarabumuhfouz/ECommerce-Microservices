using PaymentService.Application.Common.Enums;
using PaymentService.Domain.Errors;

namespace PaymentService.Application.Services;

public class ValidationService : IValidationService
{
    public Result EnsureReadyForDelivery(OrderStatus currentStatus)
    {
        if (currentStatus != OrderStatus.Shipped)
        {
            return DomainErrors.Order.InvalidStatusChange(
                current: currentStatus.ToString(),
                expected: OrderStatus.Delivered.ToString()
            );
        }

        return Result.Success();
    }

    public Result EnsurePaymentMatchesOrder(decimal paymentAmount, decimal orderAmount)
    {
        if (Math.Round(paymentAmount, 2) != Math.Round(orderAmount, 2))
        {
            return DomainErrors.Payment.AmountMismatch(
                expected: orderAmount,
                actual: paymentAmount
            );
        }
        return Result.Success();
    }

}
