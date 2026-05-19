using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems.Events;

public record StockReceivedDomainEvent(
Guid InventoryId,
Guid ProductId,
int Amount
) : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}