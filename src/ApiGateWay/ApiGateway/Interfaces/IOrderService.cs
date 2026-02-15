namespace ApiGateway.Interfaces;

public interface IOrderService
{
    Task<int> GetOrdersCountsByCustomerIdAsync(Guid customerId);
}