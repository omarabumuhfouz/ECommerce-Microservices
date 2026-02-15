using ProductService.Application.Features.Products.DTOs;
using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Products.Commands.Features.AddFeatures;
public class FeatureDtoValidator : AbstractValidator<FeatureDto>
{
    public FeatureDtoValidator()
    {
        RuleFor(x => x.Name)
            .ValidateFeatureName();

        RuleFor(x => x.Value)
            .ValidateFeatureValue();
    }
} 