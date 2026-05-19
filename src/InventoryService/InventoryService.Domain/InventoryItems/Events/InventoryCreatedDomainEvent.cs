using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems.Events;

public record InventoryCreatedDomainEvent(
Guid InventoryId,
Guid ProductId,
int InitialStock
) : IDomainEvent
{
    public Guid Id { get ; init ; } = Guid.NewGuid();
    public DateTime OccurredOn { get ; init ; } = DateTime.UtcNow;
}