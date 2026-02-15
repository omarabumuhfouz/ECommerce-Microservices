namespace ProductService.Application.Features.Products.Commands.Features.DeleteFeature;

public class DeleteFeatureFromProductCommandValidator : AbstractValidator<DeleteFeatureFromProductCommand>
{
    public DeleteFeatureFromProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.Name)
            .ValidateFeatureName();
    }
}