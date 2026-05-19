using InventoryService.Application.Features.Inventory.Queries.GetStockAvailability;

namespace InventoryService.Application.Features.Inventory.Queries.GetBatchStockAvailability;

// 1. Helper Record: Defines what the user wants for ONE item
public sealed record StockRequestItem(Guid ProductId, int RequestedQuantity);

// 2. The Batch Query: Accepts a list of these requests
public sealed record GetBatchStockAvailabilityQuery(List<StockRequestItem> Items) 
    : IQuery<List<InventoryAvailabilityDto>>;
