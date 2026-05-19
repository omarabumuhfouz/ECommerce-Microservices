using SharedKernel.Common;

namespace InventoryService.Application.Features.Inventory.Queries.GetActiveReservations;

/// <summary>
/// Handler for the <see cref="GetActiveReservationsQuery"/>.
/// </summary>
public sealed class GetActiveReservationsQueryHandler
    : IQueryHandler<GetActiveReservationsQuery, List<StockReservationDto>>
{
    private readonly  IUnitOfWork _unitOfWork;

    public GetActiveReservationsQueryHandler(IUnitOfWork unitOfWork)
         => _unitOfWork = unitOfWork;

    public async Task<Result<List<StockReservationDto>>> Handle(
        GetActiveReservationsQuery request,
        CancellationToken ct)
    {
        return await _unitOfWork.GetRepository<InventoryItem>()

        .FirstOrDefaultAsync(new InventoryItemByProductIdSpec(request.ProductId))

        .ToResult(DomainErrors.Inventory.NotFoundByProductId(request.ProductId))

        .Map(inventoryItem => inventoryItem.Reservations
            .Select(r => new StockReservationDto(
                r.OrderId,
                r.Quantity.Value,
                r.ReservedAtUtc))
            .ToList());

    }
}
