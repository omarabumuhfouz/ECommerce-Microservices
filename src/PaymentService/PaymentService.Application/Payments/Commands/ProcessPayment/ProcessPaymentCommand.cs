using PaymentService.Domain.Payments.Enums;

namespace PaymentService.Application.Payments.Commands.ProcessPayment;

public record ProcessPaymentCommand(
    Guid OrderId,
    PaymentMethod PaymentMethod,
    decimal Amount) 

: IQuery<PaymentDto>;
