namespace SharedKernel.IntegrationEvents.Inventory;

public record LowStockThresholdUpdatedIntegrationEvent(Guid InventoryId, Guid ProductId, int NewThreshold);