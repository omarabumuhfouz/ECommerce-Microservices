using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Products.Commands.ImagesManagement.AddRelatedImages;

public class AddRelatedImagesToProductCommandValidator : AbstractValidator<AddRelatedImagesToProductCommand>
{
    public AddRelatedImagesToProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.RelatedImages)
            .NotNull()
            .NotEmpty().WithMessage("At least one image must be provided.")
            .Must(images => images.Count <= ProductConstants.MAX_ADDED_IMAGES_AT_ONCE).WithMessage("You cannot add more than 10 images at once."); // Safety limit

        // Validates every item in the list using the child validator above
        RuleForEach(x => x.RelatedImages)
            .SetValidator(new ImageDtoValidator());
    }
}