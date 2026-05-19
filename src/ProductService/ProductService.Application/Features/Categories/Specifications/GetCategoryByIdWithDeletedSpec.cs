namespace ProductService.Application.Features.Categories.Specifications;

public sealed class GetCategoryByIdWithDeletedSpec : Specification<Category>
{
    public GetCategoryByIdWithDeletedSpec(Guid id, bool withTracking = false)
    {
        Query.Where(category => category.Id == id)
             .IgnoreQueryFilters();

        if (withTracking) Query.AsTracking();
    }
}