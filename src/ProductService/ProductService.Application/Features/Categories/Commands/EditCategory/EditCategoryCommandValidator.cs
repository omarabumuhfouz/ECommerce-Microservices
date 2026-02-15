namespace ProductService.Application.Features.Categories.Commands.EditCategory;

public class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
{
    public EditCategoryCommandValidator()
    {
        RuleFor(c => c.CategoryId)
            .ValidateCategoryId();

        RuleFor(c => c.Name)
        .ValidateCategoryName();

        RuleFor(c => c.Description)
            .ValidateCategoryDescription();
    }
}