namespace PaymentService.Application.Payments.Commands.MarkRefundAsComplete;

public sealed record MarkRefundAsCompleteCommand(
    Guid PaymentId,
    Guid RefundId,
    string TransactionId,
    Guid ApprovedBy) : ICommand<Unit>;
