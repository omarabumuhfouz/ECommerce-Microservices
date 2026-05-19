using Ardalis.Specification;
using ProductService.Domain.Tags;

namespace ProductService.Domain.TagManagement.Specifications;

public class GetTagsByIdsSpec : Specification<Tag>
{
    public GetTagsByIdsSpec(IEnumerable<Guid> ids)
    {
        Query.Where(c => ids.Contains(c.Id));
    }
}