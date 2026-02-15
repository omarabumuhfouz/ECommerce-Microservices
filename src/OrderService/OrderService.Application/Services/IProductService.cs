using OrderService.Domain.Orders.ValueObjects;

namespace OrderService.Application.Services;

public interface IProductService
{
    Task<Result<ProductDto>> GetProductByIdAsync(Guid id);
    Task<Result<List<ProductDto>>> GetProductsByIdsAsync(List<Guid> ids);

}
