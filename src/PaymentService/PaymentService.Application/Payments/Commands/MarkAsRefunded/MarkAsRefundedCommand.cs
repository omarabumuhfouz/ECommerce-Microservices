namespace PaymentService.Application.Payments.Commands.MarkAsRefunded;

public sealed record MarkAsRefundedCommand(Guid OrderId) : ICommand<Unit>;
