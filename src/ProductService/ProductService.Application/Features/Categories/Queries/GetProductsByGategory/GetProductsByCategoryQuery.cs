using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Categories.Queries.GetProductsByGategory;

public record GetProductsByCategoryQuery(Guid CategoryId) : IQuery<List<ProductDto>>;
