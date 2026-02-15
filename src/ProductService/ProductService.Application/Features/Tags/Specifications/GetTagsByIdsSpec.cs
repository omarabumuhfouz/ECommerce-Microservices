using ProductService.Domain.TagManagement;

namespace ProductService.Application.Features.Tags.Specifications;

public class GetTagsByIdsSpec : Specification<Tag>
{
    public GetTagsByIdsSpec(IEnumerable<Guid> ids) : base(c => ids.Contains(c.Id))
    {
    }
}