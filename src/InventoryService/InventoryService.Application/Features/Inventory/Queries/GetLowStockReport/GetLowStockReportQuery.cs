namespace InventoryService.Application.Features.Inventory.Queries.GetLowStockReport;

/// <summary>
/// DTO representing an item that is currently low on stock.
/// </summary>
/// <param name="InventoryId">The unique ID of the inventory item record.</param>
/// <param name="ProductId">The unique ID of the product.</param>
/// <param name="AvailableStock">The current physical stock on hand.</param>
/// <param name="ReservedStock">The stock currently held in reservations.</param>
/// <param name="EffectiveAvailableStock">The stock available for sale (Available - Reserved).</param>
/// <param name="Threshold">The configured low stock threshold for this item.</param>
/// <param name="Deficit">How many items below the threshold the stock is (Threshold - EffectiveAvailableStock).</param>
public sealed record LowStockItemDto(
    Guid InventoryId,
    Guid ProductId,
    int StockOnHand,
    int ReservedStock,
    int AvailableStock,
    int Threshold,
    int Deficit);

/// <summary>
/// Query to retrieve a report of all inventory items that have fallen below their configured low stock threshold.
/// </summary>
public sealed record GetLowStockReportQuery : IQuery<List<LowStockItemDto>>;
