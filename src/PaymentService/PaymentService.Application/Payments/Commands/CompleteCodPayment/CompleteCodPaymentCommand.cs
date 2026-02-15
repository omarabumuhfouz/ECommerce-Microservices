namespace PaymentService.Application.Payments.Commands.CompleteCodPayment;

public record CompleteCodPaymentCommand(
    Guid PaymentId,
    Guid OrderId) 

: ICommand<Unit>;
