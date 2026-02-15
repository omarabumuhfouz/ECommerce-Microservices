using SharedKernel.Specifications;

namespace ProductService.Domain.TagManagement.Specifications;

public class GetTagsByIdsSpec : Specification<Tag>
{
    public GetTagsByIdsSpec(IEnumerable<Guid> ids) : base(c => ids.Contains(c.Id))
    {
    }
}