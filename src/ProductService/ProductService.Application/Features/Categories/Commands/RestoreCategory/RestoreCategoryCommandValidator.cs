namespace ProductService.Application.Features.Categories.Commands.RestoreCategory;

public sealed class RestoreCategoryCommandValidator : AbstractValidator<RestoreCategoryCommand>
{
    public RestoreCategoryCommandValidator()
    {
        RuleFor(c => c.CategoryId)
        .ValidateCategoryId();
    }
}