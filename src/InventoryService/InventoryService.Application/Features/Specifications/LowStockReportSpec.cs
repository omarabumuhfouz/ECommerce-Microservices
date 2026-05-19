using Ardalis.Specification;
using InventoryService.Application.Features.Inventory.Queries.GetLowStockReport;
using InventoryService.Domain.InventoryItems;

namespace InventoryService.Application.Features.Specifications;

public sealed class LowStockReportSpec : Specification<InventoryItem, LowStockItemDto>
{
    public LowStockReportSpec()
    {
        // 1. Filter: Find items where available stock is less than or equal to the reorder point
        Query.Where(item => item.AvailableStock.Value <= item.LowStockThreshold);

        // 2. Performance: Read-only query
        Query.AsNoTracking();

        // 3. Projection: Map directly to the DTO
        Query.Select(item => new LowStockItemDto(
            item.Id,
            item.ProductId,
            item.StockOnHand,                                 // Available Stock
            item.ReservedStock,                 // Reserved Stock (Sum from child collection)
            item.CurrentAvailableStock,                                 // Effective Stock
            item.LowStockThreshold,                                       // Threshold
            item.Deficit         // Deficit (How many to buy)
        ));
    }
}