using ProductService.Domain.TagManagement;

namespace ProductService.Application.Features.Tags.Specifications;
public class GetTagByIdSpec : Specification<Tag>
{
    public GetTagByIdSpec(Guid tagId) : base(t => t.Id == tagId)
    {
    }
}