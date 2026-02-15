using SharedKernel.Shared;
using OrderService.Domain.Errors;

namespace OrderService.Domain.Orders.Enums;

public static class OrderStateMachine
{
    // Using HashSet for O(1) lookup performance
    private static readonly Dictionary<OrderStatus, HashSet<OrderStatus>> TransitionMap = new()
    {
        { OrderStatus.Pending,    new() { OrderStatus.Processing, OrderStatus.Canceled } },
        { OrderStatus.Processing, new() { OrderStatus.Shipped, OrderStatus.Canceled, OrderStatus.Refunded} },
        { OrderStatus.Shipped,    new() { OrderStatus.Delivered, OrderStatus.Refunded } },
        { OrderStatus.Delivered,  new() { OrderStatus.Refunded } },
        { OrderStatus.Canceled,   new() },
        { OrderStatus.Refunded,   new() }
    };

    public static Result EnsureValidTransition(OrderStatus current, OrderStatus next)
    {
        if (!TransitionMap.TryGetValue(current, out var allowedNextStatuses))
            return DomainErrors.Order.StatusNotConfigured(current);

        if (!allowedNextStatuses.Contains(next))
            return DomainErrors.Order.InvalidTransition(current, next);

        return Result.Success();
    }
}