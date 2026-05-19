namespace SharedKernel.IntegrationEvents.Inventory;

public record InventoryAdjustedIntegrationEvent(
    Guid InventoryItemId,
    Guid ProductId,
    Guid OrderId,
    int QuantityDelta, // Positive (+) if stock was reserved/added, Negative (-) if released/removed
    DateTime OccurredOn
);