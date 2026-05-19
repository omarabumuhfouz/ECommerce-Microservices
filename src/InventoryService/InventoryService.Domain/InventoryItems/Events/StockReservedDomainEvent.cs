using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems.Events;

public record StockReservedDomainEvent(
Guid InventoryId,
Guid ProductId,
Guid OrderId,
int Quantity
) : IDomainEvent
{
    public Guid Id { get ; init ; } = Guid.NewGuid();
    public DateTime OccurredOn { get ; init ; } = DateTime.UtcNow;
}