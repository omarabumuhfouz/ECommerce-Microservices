using OrderService.Domain.Orders.Events;
using SharedKernel.Primitives;

namespace OrderService.Domain.Orders.Events;

public record OrderProcessingDomainEvent(
  Guid OrderId,
  Guid CustomerId,
  List<OrderEventItem> Items
)
: IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
