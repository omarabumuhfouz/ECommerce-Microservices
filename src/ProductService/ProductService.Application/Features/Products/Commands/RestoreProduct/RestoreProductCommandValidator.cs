namespace ProductService.Application.Features.Products.Commands.RestoreProduct;

public sealed class RestoreProductCommandValidator : AbstractValidator<RestoreProductCommand>
{
    public RestoreProductCommandValidator()
    {
        RuleFor(t => t.ProductId)
        .ValidateProductId();
    }
}