namespace ProductService.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .ValidateCategoryName();

        RuleFor(x => x.Description)
             .ValidateCategoryDescription();
    }
}
