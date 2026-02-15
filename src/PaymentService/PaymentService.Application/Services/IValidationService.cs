using PaymentService.Application.Common.Enums;

namespace PaymentService.Application.Services;

public interface IValidationService
{
    Result EnsureReadyForDelivery(OrderStatus status);

    Result EnsurePaymentMatchesOrder(decimal paymentAmount, decimal orderAmount);

}
