namespace InventoryService.Application.Features.Inventory.Queries.GetActiveReservations;

/// <summary>
/// DTO representing an active reservation of stock.
/// </summary>
/// <param name="OrderId">The unique identifier for the order holding the reservation.</param>
/// <param name="Quantity">The amount of stock being reserved.</param>
/// <param name="ReservedAtUtc">The timestamp when the reservation was created.</param>
public sealed record StockReservationDto(
    Guid OrderId,
    int Quantity,
    DateTime ReservedAtUtc);

/// <summary>
/// Query to retrieve all active stock reservations for a specific product.
/// </summary>
/// <param name="ProductId">The product ID to query for reservations.</param>
public sealed record GetActiveReservationsQuery(
    Guid ProductId) : IQuery<List<StockReservationDto>>;
