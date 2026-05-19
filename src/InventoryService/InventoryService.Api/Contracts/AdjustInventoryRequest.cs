namespace InventoryService.Api.Endpoints;

public record AdjustInventoryRequest(Guid OrderId,int Quantity);
