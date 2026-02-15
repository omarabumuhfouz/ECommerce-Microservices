using OrderService.Domain.Orders.Events;

namespace OrderService.Domain.Orders.Events;

public record OrderRefundedDomainEvent(
  Guid OrderId,
  Guid CustomerId,
List<OrderEventItem> Items
)
: IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}