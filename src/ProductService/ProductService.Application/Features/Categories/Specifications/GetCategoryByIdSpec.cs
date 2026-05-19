
namespace ProductService.Application.Features.Categories.Specifications;

public sealed class GetCategoryByIdSpec : Specification<Category>
{
    public GetCategoryByIdSpec(Guid id, bool withTracking = false)
    {
        Query.Where(category => category.Id == id);

        if (withTracking) Query.AsTracking();
    }
}