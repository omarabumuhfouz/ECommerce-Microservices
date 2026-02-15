using OrderService.Application.Orders.DTOs;

namespace OrderService.Application.Orders.Commands.AddOrderItem;

public record AddOrderItemCommand(
    Guid OrderId,
    Guid ProductId,
    string ProductName,
    int Quantity

) : ICommand<Unit>;