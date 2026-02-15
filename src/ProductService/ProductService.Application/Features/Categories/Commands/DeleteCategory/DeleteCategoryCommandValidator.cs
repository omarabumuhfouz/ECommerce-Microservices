namespace ProductService.Application.Features.Categories.Commands.DeleteCategory;
public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {

        RuleFor(c => c.CategoryId)
            .ValidateCategoryId();
    }
}