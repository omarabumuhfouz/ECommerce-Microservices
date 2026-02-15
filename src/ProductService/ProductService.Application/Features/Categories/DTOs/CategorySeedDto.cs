using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Categories.DTOs;

public class CategorySeedDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public List<ProductSeedDto> Products { get; set; } = new();
}
