using SharedKernel.Specifications;

namespace ProductService.Application.Features.Categories.Specifications;

public class GetCategoriesSpec : Specification<Category>
{
    public GetCategoriesSpec() : base(c => c.IsActive == true)
    {
    }
}