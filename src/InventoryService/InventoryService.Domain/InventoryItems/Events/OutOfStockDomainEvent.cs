using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems.Events;

public record OutOfStockDomainEvent(
    Guid InventoryId,
    Guid ProductId

) : IDomainEvent
{
    public Guid Id { get ; init ; } = Guid.NewGuid();
    public DateTime OccurredOn { get ; init ; } = DateTime.Now;
}