using InventoryService.Application.Features.Specifications;
using SharedKernel.Abstractions.Data;

namespace InventoryService.Application.Features.Inventory.Queries.GetLowStockReport;

public sealed class GetLowStockReportQueryHandler 
    : IQueryHandler<GetLowStockReportQuery, List<LowStockItemDto>>
{
    private readonly IRepository<InventoryItem> _repository;

    public GetLowStockReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _repository = unitOfWork.GetRepository<InventoryItem>();
    }

    public async Task<Result<List<LowStockItemDto>>> Handle(
        GetLowStockReportQuery request, 
        CancellationToken cancellationToken)
    {
        var lowStockItems = await _repository.GetListAsync(new LowStockReportSpec(), cancellationToken);

        return Result.Success(lowStockItems);
    }
}
