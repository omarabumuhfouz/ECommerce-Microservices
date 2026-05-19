namespace SharedKernel.IntegrationEvents.Inventory;

public record LowStockThresholdReachedIntegrationEvent(Guid InventoryId, Guid ProductId, int CurrentAvailableStock);