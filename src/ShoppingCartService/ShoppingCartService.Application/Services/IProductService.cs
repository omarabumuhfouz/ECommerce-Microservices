namespace ShoppingCartService.Application.Services;

public interface IProductService
{
    Task<Result<ProductDto>> GetProductByIdAsync(Guid productId);
    Task<Result<List<ProductNameDto>>> GetProductNamesByIdsAsync(List<Guid> productIds);
}
