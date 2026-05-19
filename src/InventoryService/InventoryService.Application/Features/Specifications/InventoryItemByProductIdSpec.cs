using Ardalis.Specification;

namespace InventoryService.Application.Features.Inventory.Specifications;

public sealed class InventoryItemByProductIdSpec : Specification<InventoryItem>, ISingleResultSpecification<InventoryItem>
{
    public InventoryItemByProductIdSpec(Guid productId,bool withTracking = false)
    {
        if (withTracking) Query.AsTracking();

        Query
            .Where(item => item.ProductId == productId)
            .Include(item => item.Reservations);
    }
}