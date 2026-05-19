namespace InventoryService.Application.Features.Inventory.Queries.GetStockAvailability;

/// <summary>
/// DTO representing the stock availability of a single product.
/// </summary>
/// <param name="ProductId">The unique identifier for the product.</param>
/// <param name="IsInStock">Indicates if there is any stock available (Available > 0).</param>
/// <param name="CanFulfill">Indicates if the available stock can fulfill the requested quantity.</param>
/// <param name="AvailableStock">The current available stock level (Available - Reserved).</param>
public sealed record InventoryAvailabilityDto(
    Guid ProductId,
    bool IsInStock,
    bool CanFulfill,
    int AvailableStock);

/// <summary>
/// Query to check the stock availability for a single product against a requested quantity.
/// </summary>
/// <param name="ProductId">The ID of the product to check.</param>
/// <param name="RequestedQuantity">The quantity the user wishes to order.</param>
public sealed record GetStockAvailabilityQuery(
    Guid ProductId,
    int RequestedQuantity) : IQuery<InventoryAvailabilityDto>;
