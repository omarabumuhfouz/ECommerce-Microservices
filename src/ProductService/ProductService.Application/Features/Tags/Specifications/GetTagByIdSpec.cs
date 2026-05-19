using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Specifications;
public class GetTagByIdSpec : Specification<Tag>
{
    public GetTagByIdSpec(Guid tagId)
    {
        Query.Where(tag => tag.Id == tagId);
    }
}