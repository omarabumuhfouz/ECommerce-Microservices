using System.ComponentModel;

namespace ProductService.Application.Features.Products.Specifications;
public class GetProductByIdSpec : Specification<Product>
{
    public GetProductByIdSpec(Guid id, bool withTracking = false) : base(p => p.Id == id)
    {
        AddInclude(p => p.Tags);

        if (withTracking) EnableTracking();
    }
}