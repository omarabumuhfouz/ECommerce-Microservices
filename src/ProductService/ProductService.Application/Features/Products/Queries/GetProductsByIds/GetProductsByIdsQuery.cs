using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProductsByIds;

public record GetProductsByIdsQuery(List<Guid> ProductIds) : IQuery<List<ProductDetailsDto>>;
