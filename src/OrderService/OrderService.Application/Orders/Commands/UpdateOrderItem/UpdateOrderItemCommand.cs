namespace OrderService.Application.Orders.Commands.UpdateOrderItem;

public record UpdateOrderItemCommand(
    Guid OrderId,
    Guid ItemId,
    int Quantity
    ) : ICommand<Unit>;