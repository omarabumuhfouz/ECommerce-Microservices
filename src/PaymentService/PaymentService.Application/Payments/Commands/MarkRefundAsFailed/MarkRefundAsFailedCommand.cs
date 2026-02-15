namespace PaymentService.Application.Payments.Commands.MarkRefundAsFailed;

public sealed record MarkRefundAsFailedCommand(
    Guid PaymentId,
    Guid RefundId) : ICommand<Unit>;
