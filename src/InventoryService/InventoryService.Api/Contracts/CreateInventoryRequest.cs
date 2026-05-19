namespace InventoryService.Api.Endpoints;

public record CreateInventoryRequest(Guid ProductId, int InitialStock, int LowStockThreshold);
