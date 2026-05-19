using Ardalis.Specification;

namespace InventoryService.Application.Features.Specifications;

public sealed class InventoryBatchStockSpec : Specification<InventoryItem>
{
    public InventoryBatchStockSpec(List<Guid> productIds)
    {
        Query.Where(item => productIds.Contains(item.ProductId));

        Query.AsNoTracking();

    }
}