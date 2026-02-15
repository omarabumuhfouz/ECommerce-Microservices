namespace PaymentService.Application.Payments.Commands.MarkAsFailed;

public sealed record MarkAsFailedCommand(Guid PaymentId) : ICommand<Unit>;
