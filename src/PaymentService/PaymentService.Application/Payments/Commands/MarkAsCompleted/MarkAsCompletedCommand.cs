namespace PaymentService.Application.Payments.Commands.MarkAsCompleted;

public sealed record MarkAsCompletedCommand(Guid PaymentId, string? TransactionId) : ICommand<Unit>;
