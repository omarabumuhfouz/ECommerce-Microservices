using SharedKernel.Specifications;

namespace ProductService.Application.Features.Products.Specifications;

public class GetProductsByCategoryIdSpec : Specification<Product>
{
    public GetProductsByCategoryIdSpec(Guid categoryId) : base(p => p.CategoryId == categoryId)
    {
        AddInclude(p => p.Tags);
    }
}