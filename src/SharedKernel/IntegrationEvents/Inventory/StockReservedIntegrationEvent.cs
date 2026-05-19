namespace SharedKernel.IntegrationEvents.Inventory;

public record StockReservedIntegrationEvent(Guid InventoryId, Guid ProductId, Guid OrderId, int Amount);