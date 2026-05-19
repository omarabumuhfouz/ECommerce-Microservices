namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;

public sealed record UpdateOrderStatusToCanceledCommand(Guid OrderId) : ICommand<Unit>;