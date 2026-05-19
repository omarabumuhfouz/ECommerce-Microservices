namespace ProductService.Application.Features.Products.Specifications;

public sealed class GetProductByIdWithDeletedSpec : Specification<Product>
{
    public GetProductByIdWithDeletedSpec(Guid id, bool withTracking = false)
    {
        Query.Where(category => category.Id == id)
             .IgnoreQueryFilters();

        if (withTracking) Query.AsTracking();
    }
}