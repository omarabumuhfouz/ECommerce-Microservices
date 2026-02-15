namespace ProductService.Api.Contracts.Product;

public record GetProductsRequest(int PageNumber=1, int PageSize=10, bool IsAscending=true, string? SortBy = null);