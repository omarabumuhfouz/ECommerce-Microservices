using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Products.Commands.Features.AddFeatures;
public class AddFeaturesToProductCommandValidator : AbstractValidator<AddFeaturesToProductCommand>
{
    public AddFeaturesToProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.Features)
            .NotEmpty().WithMessage("At least one feature must be provided.") // Checks for null or empty list
            .Must(f => f.Count <= ProductConstants.MaxAddedFeaturesAtOnce).WithMessage("You cannot add more than 20 features at once."); // Optional limit

        // The "RuleForEach" validates every element in the list
        RuleForEach(x => x.Features)
            .SetValidator(new FeatureDtoValidator());
    }
}