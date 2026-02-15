namespace OrderService.Application.Orders.Commands.ExpireOrder;
public record ExpireOrderCommand(Guid OrderId) : ICommand<Unit>;