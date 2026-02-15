namespace CancellationService.Application.Services;

public interface IOrderService
{
    Task<Result<OrderDto>> GetOrderByIdAsync(Guid orderId);
}
