using SharedKernel.Specifications;

namespace ProductService.Domain.TagManagement.Specifications;
public class GetTagByIdSpec : Specification<Tag>
{
    public GetTagByIdSpec(Guid tagId) : base(t => t.Id == tagId)
    {
    }
}