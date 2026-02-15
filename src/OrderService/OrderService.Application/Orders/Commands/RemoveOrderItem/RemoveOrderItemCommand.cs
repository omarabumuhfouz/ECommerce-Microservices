namespace OrderService.Application.Orders.Commands.RemoveOrderItem;

public record RemoveOrderItemCommand(
    Guid OrderId,
    Guid ItemId
    ) : ICommand<Unit>;