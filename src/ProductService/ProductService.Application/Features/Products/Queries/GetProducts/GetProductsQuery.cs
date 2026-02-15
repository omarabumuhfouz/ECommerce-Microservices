using ProductService.Application.Features.Products.DTOs;
using SharedKernel.Common;

namespace ProductService.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(PagingParams PagingParams) : IQuery<PagedList<ProductDto>>;
