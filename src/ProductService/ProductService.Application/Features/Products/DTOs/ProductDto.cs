using Contracts.Product; // For your gRPC model if needed

namespace ProductService.Application.Features.Products.DTOs;

// LIGHTWEIGHT: Use this for Lists/Search Results
public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public ImageDto MainImage { get; set; }
    public int DiscountPercentage { get; set; }
    public bool IsAvailable { get; set; }
    
    // Minimal method for gRPC lists
    // public ProductModel ToGrpcModel()
    // {
    //     return new ProductModel
    //     {
    //         Id = this.Id.ToString(),
    //         Name = this.Name,
    //         DiscountPercentage = this.DiscountPercentage,
    //         Price = this.Price.ToString(),
    //         IsAvailable = this.IsAvailable,
    //         StockQuantity = 0 // Not needed for lists usually
    //     };
    // }
}

// HEAVY: Use this for the "Single Product View" page
