namespace ProductService.Application.Features.Products.Specifications;

public class GetProductsByCategoryIdSpec : Specification<Product>
{
    public GetProductsByCategoryIdSpec(Guid categoryId)
    {
        Query.Where(p => p.CategoryId == categoryId)
        .Include(p => p.Tags);
    }
}