namespace ProductService.Application.Features.Products.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public ImageDto MainImage { get; set; } = ImageDto.Empty();
    public string Description { get; set; } = string.Empty;
    public int DiscountPercentage { get; set; }
}