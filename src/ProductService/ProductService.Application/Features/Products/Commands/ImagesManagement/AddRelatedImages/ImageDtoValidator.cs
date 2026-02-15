using ProductService.Application.Features.Products.DTOs;
using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Products.Commands.ImagesManagement.AddRelatedImages;

public class ImageDtoValidator : AbstractValidator<ImageDto>
{
    public ImageDtoValidator()
    {
        RuleFor(x => x.Url)
            .ValidateImageUrl();
            
        RuleFor(x => x.AltText)
            .ValidateImageAltText();
    }
}