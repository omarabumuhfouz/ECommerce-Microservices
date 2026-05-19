namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToShipped;

public sealed record UpdateOrderStatusToShippedCommand(Guid OrderId) : ICommand<Unit>;