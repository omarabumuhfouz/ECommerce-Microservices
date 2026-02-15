using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Products.Commands.Features.EditFeature;
public class EditFeatureFromProductCommandValidator : AbstractValidator<EditFeatureFromProductCommand>
{
    public EditFeatureFromProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.OldName)
            .NotEmpty().WithMessage("Old feature name is required.")
            .MaximumLength(ProductConstants.FEATURE_NAME_MAX_LENGTH).WithMessage("Old feature name must not exceed 100 characters.");

        RuleFor(x => x.NewName)
            .NotEmpty().WithMessage("New feature name is required.")
            .MaximumLength(ProductConstants.FEATURE_NAME_MAX_LENGTH).WithMessage("New feature name must not exceed 100 characters.");

        RuleFor(x => x.NewValue)
            .NotEmpty().WithMessage("New feature value is required.")
            .MaximumLength(ProductConstants.FEATURE_VALUE_MAX_LENGTH).WithMessage("New feature value must not exceed 200 characters.");
    }
}