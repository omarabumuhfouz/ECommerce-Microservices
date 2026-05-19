using Ardalis.Specification;

namespace ProductService.Domain.Tags.Specifications;
public class GetTagByIdSpec : Specification<Tag>
{
    public GetTagByIdSpec(Guid tagId)
    {
        Query.Where(t => t.Id == tagId);
    }
}