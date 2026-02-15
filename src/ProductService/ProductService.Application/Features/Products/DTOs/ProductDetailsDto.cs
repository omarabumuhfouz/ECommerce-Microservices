using Contracts.Product;

namespace ProductService.Application.Features.Products.DTOs;

public record ProductDetailsDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public string Currency { get; init; }
    public int StockQuantity { get; init; }
    public bool IsAvailable { get; init; }
    public Guid CategoryId { get; init; }

    // Discount Info
    public int DiscountPercentage { get; init; }
    public DateTime? DiscountEndDate { get; init; }

    // Complex Data
    public ImageDto MainImage { get; init; }
    public List<ImageDto> RelatedImages { get; init; } = new();
    public List<FeatureDto> Features { get; init; } = new();
    public List<string> Tags { get; init; } = new();

    public ProductModel ToGrpcModel()
    {
        return new ProductModel
        {
            Id = Id.ToString(),
            Name = Name,
            Price = Price.ToString(System.Globalization.CultureInfo.InvariantCulture),
            StockQuantity = StockQuantity,
            IsAvailable = IsAvailable,
            DiscountPercentage = DiscountPercentage
        };
    }
}