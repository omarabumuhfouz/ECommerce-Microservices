namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToRefunded;

public sealed record UpdateOrderStatusToRefundedCommand(Guid OrderId) : ICommand<Unit>;
