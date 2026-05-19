using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Specifications;
public class GetProductByIdSpec : Specification<Product>
{
    public GetProductByIdSpec(Guid id, bool withTracking = false) 
    {
        Query.Where(p => p.Id == id)
        .Include(p => p.Tags);

        if (withTracking) Query.AsTracking();
    }
}