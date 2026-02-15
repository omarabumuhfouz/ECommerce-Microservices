namespace PaymentService.Application.Payments.Commands.MarkAsPending;

public sealed record MarkAsPendingCommand(Guid PaymentId) : ICommand<Unit>;
