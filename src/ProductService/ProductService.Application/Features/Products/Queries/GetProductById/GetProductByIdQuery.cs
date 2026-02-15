using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<ProductDetailsDto>;
