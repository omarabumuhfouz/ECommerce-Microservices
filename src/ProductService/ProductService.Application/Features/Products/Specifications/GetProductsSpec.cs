using System.Linq.Expressions;
using ProductService.Application.Features.Products.DTOs;
using ProductService.Domain.Constants;
using SharedKernel.Common;

namespace ProductService.Application.Features.Products.Specifications;

public class GetProductsSpec : Specification<Product>
{
    public GetProductsSpec(PagingParams pagingParams) : base(p => p.IsAvailable)
    {
        if (pagingParams is null) pagingParams = PagingParams.Default;

        Expression<Func<Product, object>> keySelector = pagingParams.SortBy?.ToLower() switch
        {
            ProductConstants.SEARCH_BY_NAME => p => p.Name,
            ProductConstants.SEARCH_BY_PRICE => p => p.Price,
            _ => p => p.Name
        };

        if (pagingParams.IsAscending) AddOrderBy(keySelector);
        else AddOrderByDescending(keySelector);

        ApplyPaging(pagingParams.Skip, pagingParams.PageSize);
    }
}