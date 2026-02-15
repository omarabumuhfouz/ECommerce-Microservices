using SharedKernel.Primitives;

namespace OrderService.Domain.Orders.Events;

public record ItemRemovedDomainEvent(
    Guid OrderId,
    Guid ProductId,
    int QuantityRemoved
) : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}