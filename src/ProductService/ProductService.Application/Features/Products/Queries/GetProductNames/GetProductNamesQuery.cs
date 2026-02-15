using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProductNames;

public record GetProductNamesQuery(List<Guid> ProductIds) : IQuery<List<ProductNameDto>>;

