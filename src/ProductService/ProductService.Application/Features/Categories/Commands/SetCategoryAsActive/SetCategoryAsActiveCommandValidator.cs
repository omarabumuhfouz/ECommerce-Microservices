namespace ProductService.Application.Features.Categories.Commands.SetCategoryAsActive;

public class SetCategoryAsActiveCommandValidator : AbstractValidator<SetCategoryAsActiveCommand>
{
    public SetCategoryAsActiveCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .ValidateCategoryId();
    }
}
