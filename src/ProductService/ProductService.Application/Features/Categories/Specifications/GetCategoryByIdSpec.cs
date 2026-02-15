using SharedKernel.Specifications;

namespace ProductService.Application.Features.Categories.Specifications;

public class GetCategoryByIdSpec : Specification<Category>
{
    public GetCategoryByIdSpec(Guid id) : base(c => c.Id == id)
    {
    }
}