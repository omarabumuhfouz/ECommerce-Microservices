namespace InventoryService.Api.Endpoints;

public record ReserveStockRequest(Guid OrderId, int Quantity);
