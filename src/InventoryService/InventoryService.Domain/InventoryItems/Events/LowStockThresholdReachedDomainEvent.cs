using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems.Events;

public record LowStockThresholdReachedDomainEvent(
    Guid InventoryId,
    Guid ProductId,
    int AvailableQuantity

) : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}