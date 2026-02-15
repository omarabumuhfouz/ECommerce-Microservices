namespace ProductService.Application.Features.Products.Commands.ImagesManagement.ReplaceMainImage;

public class ReplaceMainImageCommandValidator : AbstractValidator<ReplaceMainImageCommand>
{
    public ReplaceMainImageCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .ValidateProductId();

        RuleFor(c => c.NewMainUrl)
            .ValidateImageUrl();

        RuleFor(c => c.NewTextAlt)
            .ValidateImageAltText();
    }
}