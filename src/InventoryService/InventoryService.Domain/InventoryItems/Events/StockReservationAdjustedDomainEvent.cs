using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems.Events;

public sealed record StockReservationAdjustedDomainEvent(
    Guid InventoryItemId,
    Guid ProductId,
    Guid OrderId,
    int QuantityDelta // Positive (+) for Add, Negative (-) for Remove
) : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}