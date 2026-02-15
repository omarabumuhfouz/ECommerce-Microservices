namespace PaymentService.Application.Services;

public interface IOrderService
{
    Task<Result<OrderDto>> GetOrderByIdAsync(Guid id);
}
