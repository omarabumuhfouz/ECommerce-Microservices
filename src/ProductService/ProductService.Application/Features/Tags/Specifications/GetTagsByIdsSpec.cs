using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Specifications;

public class GetTagsByIdsSpec : Specification<Tag>
{
    public GetTagsByIdsSpec(IEnumerable<Guid> ids)
    {
        Query.Where(t => ids.Contains(t.Id));
    }
}