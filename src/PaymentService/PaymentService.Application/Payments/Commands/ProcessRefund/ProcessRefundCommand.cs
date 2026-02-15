namespace PaymentService.Application.Payments.Commands.ProcessRefund;

public record ProcessRefundCommand(
    Guid OrderId,
    Guid CancellationId,
    Guid ApprovedByUserId,
    decimal Amount,
    string Remarks) : ICommand<Unit>;