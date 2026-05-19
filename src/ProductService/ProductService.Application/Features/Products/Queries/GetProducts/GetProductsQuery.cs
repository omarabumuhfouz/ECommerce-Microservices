using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(PagingParams PagingParams) : IQuery<PagedList<ProductDto>>;
