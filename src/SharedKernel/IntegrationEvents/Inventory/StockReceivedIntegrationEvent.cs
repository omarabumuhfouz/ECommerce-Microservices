namespace SharedKernel.IntegrationEvents.Inventory;

public record StockReceivedIntegrationEvent(Guid InventoryId, Guid ProductId, int Amount);