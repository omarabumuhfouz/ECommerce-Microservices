namespace ProductService.Application.Features.Products.DTOs;

public class ProductSeedDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int DiscountPercentage { get; set; }
    public string ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public int CategoryId { get; set; }
}
