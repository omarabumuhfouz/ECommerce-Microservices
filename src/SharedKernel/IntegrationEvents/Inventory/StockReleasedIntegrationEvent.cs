namespace SharedKernel.IntegrationEvents.Inventory;

public record StockReleasedIntegrationEvent(Guid InventoryId, Guid ProductId, Guid OrderId);