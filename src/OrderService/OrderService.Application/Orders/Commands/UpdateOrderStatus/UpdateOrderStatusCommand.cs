using OrderService.Domain.Orders.Enums;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand(
    Guid OrderId,
    OrderStatus OrderStatus
    )
    : ICommand<Unit>;
