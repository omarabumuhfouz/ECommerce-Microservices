namespace SharedKernel.IntegrationEvents.Inventory;

public record StockDispatchedIntegrationEvent(Guid InventoryId, Guid ProductId, Guid OrderId);