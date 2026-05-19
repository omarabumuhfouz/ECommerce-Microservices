using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems.Events;

public record ReservationReleasedDomainEvent(
    Guid InventoryId,
    Guid ProductId,
    Guid OrderId

) : IDomainEvent
{
    public Guid Id { get ; init ; } = Guid.NewGuid();
    public DateTime OccurredOn { get ; init ; } = DateTime.Now;
}
// RaiseDomainEvent(new ReservationReleasedDomainEvent(Id, ProductId, orderId));