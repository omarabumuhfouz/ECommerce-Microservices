using ProductService.Application.Features.Categories.Queries.GetProductsByGategory;

namespace ProductService.Application.Validators.Categories;

public class GetProductsByCategoryQueryValidator : AbstractValidator<GetProductsByCategoryQuery>
{
    public GetProductsByCategoryQueryValidator()
    {
        RuleFor(c => c.CategoryId)
            .ValidateCategoryId();
    }
}