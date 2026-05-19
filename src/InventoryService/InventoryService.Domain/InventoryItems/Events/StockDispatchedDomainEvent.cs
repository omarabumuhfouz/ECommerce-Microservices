using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems.Events;

public record StockDispatchedDomainEvent(
    Guid InventoryId,
    Guid ProductId,
    Guid OrderId,
    int Quantity

) : IDomainEvent
{
    public Guid Id { get ; init ; } = Guid.NewGuid();
    public DateTime OccurredOn { get ; init ; } = DateTime.Now;
}

// RaiseDomainEvent(new StockDispatchedDomainEvent(Id, ProductId, orderId, reservation.Quantity.Value));