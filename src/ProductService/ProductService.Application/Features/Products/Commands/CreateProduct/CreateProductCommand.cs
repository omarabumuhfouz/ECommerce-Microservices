using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    Guid CategoryId,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    ImageDto MainImage,
    int DiscountPercentage,
    DateTime? DiscountEndDate,
    List<ImageDto>? RelatedImages,
    List<FeatureDto>? Features,
    List<Guid>? TagIds
) : ICommand<Guid>;
