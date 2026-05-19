namespace SharedKernel.IntegrationEvents.Inventory;

public record InventoryCreatedIntegrationEvent(Guid InventoryId, Guid ProductId, int InitialStock);