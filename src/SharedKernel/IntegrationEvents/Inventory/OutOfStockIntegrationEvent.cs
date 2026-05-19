namespace SharedKernel.IntegrationEvents.Inventory;

public record OutOfStockIntegrationEvent(Guid InventoryId, Guid ProductId);