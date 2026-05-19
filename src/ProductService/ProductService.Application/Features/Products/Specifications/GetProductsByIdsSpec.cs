using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Specifications;

public class GetProductsByIdsSpec : Specification<Product>
{
    public GetProductsByIdsSpec(List<Guid> ids) : base(p => ids.Contains(p.Id))
    {
        AddInclude(p => p.Tags);
    }
}
