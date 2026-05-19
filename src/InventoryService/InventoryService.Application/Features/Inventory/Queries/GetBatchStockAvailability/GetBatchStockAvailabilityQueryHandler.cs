using InventoryService.Application.Abstractions;
using InventoryService.Application.Features.Inventory.Queries.GetStockAvailability;
using InventoryService.Application.Features.Specifications;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions.Data;

namespace InventoryService.Application.Features.Inventory.Queries.GetBatchStockAvailability;

/// <summary>
/// Handler for the <see cref="GetBatchStockAvailabilityQuery"/>.
/// </summary>
public sealed class GetBatchStockAvailabilityQueryHandler
    : IQueryHandler<GetBatchStockAvailabilityQuery, List<InventoryAvailabilityDto>>
{
    private readonly IRepository<InventoryItem> _repository;

    public GetBatchStockAvailabilityQueryHandler(IUnitOfWork unitOfWork)
         => _repository = unitOfWork.GetRepository<InventoryItem>();


    public async Task<Result<List<InventoryAvailabilityDto>>> Handle(
    GetBatchStockAvailabilityQuery request, 
    CancellationToken ct)
{
    // 1. Fetch
    var spec = new InventoryBatchStockSpec(request.Items.Select(x => x.ProductId).Distinct().ToList());
    var dbItems = await _repository.GetListAsync(spec, ct);

    // 2. Map (Using Domain Logic)
    var result = request.Items.Select(reqItem => 
    {
        var dbItem = dbItems.FirstOrDefault(x => x.ProductId == reqItem.ProductId);

        if (dbItem is null)
        {
            // Case A: Item Missing
            return new InventoryAvailabilityDto(
                reqItem.ProductId, 
                IsInStock: false, 
                CanFulfill: false, 
                AvailableStock: 0
            );
        }

        // Case B: Item Found
        return new InventoryAvailabilityDto(
            reqItem.ProductId,
            IsInStock: dbItem.IsInStock,
            CanFulfill: dbItem.CanFulfill(reqItem.RequestedQuantity),
            AvailableStock: dbItem.CurrentAvailableStock
        );
    }).ToList();

    return Result.Success(result);
}
}
