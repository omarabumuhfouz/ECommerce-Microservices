using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Specifications;

public sealed class GetTagByIdWithDeletedSpec : Specification<Tag>
{
    public GetTagByIdWithDeletedSpec(Guid id, bool withTracking = false)
    {
        Query.Where(category => category.Id == id)
             .IgnoreQueryFilters();

        if (withTracking) Query.AsTracking();
    }
}