using InventoryService.Application.Abstractions;
using InventoryService.Application.Features.Inventory.Specifications;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Data;
using SharedKernel.Common;

namespace InventoryService.Application.Features.Inventory.Queries.GetStockAvailability;

/// <summary>
/// Handler for the <see cref="GetStockAvailabilityQuery"/>.
/// </summary>
public sealed class GetStockAvailabilityQueryHandler
    : IQueryHandler<GetStockAvailabilityQuery, InventoryAvailabilityDto>
{
    private readonly IRepository<InventoryItem> _repository;


    public GetStockAvailabilityQueryHandler(IUnitOfWork unitOfWork)
        => _repository = unitOfWork.GetRepository<InventoryItem>();



    public async Task<Result<InventoryAvailabilityDto>> Handle(
        GetStockAvailabilityQuery request,
        CancellationToken ct)
    {
        return await _repository
             .FirstOrDefaultAsync(new InventoryItemByProductIdSpec(request.ProductId), ct)
             .ToResult(DomainErrors.Inventory.NotFoundByProductId(request.ProductId))
             .Map(inventoryItem =>
                    new InventoryAvailabilityDto(
                        request.ProductId,
                        inventoryItem.IsInStock,
                        inventoryItem.CanFulfill(request.RequestedQuantity),
                        inventoryItem.CurrentAvailableStock));
    }
}
