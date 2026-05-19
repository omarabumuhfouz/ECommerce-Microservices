using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Tags.Queries.GetProductsByTagIdQuery;
public sealed record GetProductsByTagIdQuery(Guid TagId) : ICommand<List<ProductDto>>;