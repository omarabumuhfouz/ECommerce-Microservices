namespace ProductService.Application.Features.Products.Commands.ImagesManagement.ReplaceRelatedImage;

public class ReplaceRelatedImageCommandValidator : AbstractValidator<ReplaceRelatedImageCommand>
{
    public ReplaceRelatedImageCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .ValidateProductId();

        RuleFor(c => c.OldUrl)
            .ValidateImageUrl();

        RuleFor(c => c.NewUrl)
            .ValidateImageUrl();

        RuleFor(c => c.NewAltText)
            .ValidateImageAltText();
    }

}