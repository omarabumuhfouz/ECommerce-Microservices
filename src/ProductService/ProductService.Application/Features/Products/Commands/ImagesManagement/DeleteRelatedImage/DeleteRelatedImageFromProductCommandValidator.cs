namespace ProductService.Application.Features.Products.Commands.ImagesManagement.DeleteRelatedImage;
public class DeleteRelatedImageFromProductCommandValidator : AbstractValidator<DeleteRelatedImageFromProductCommand>
{
    public DeleteRelatedImageFromProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.Url)
            .ValidateImageUrl();
    }
}