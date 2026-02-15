using Ardalis.Specification;
using OrderService.Application.Orders.Mappings;
using OrderService.Domain.Orders;

namespace OrderService.Application.Orders.Specifications;

public class GetOrdersByCustomerSpec : Specification<Order, OrderDto>
{
    public GetOrdersByCustomerSpec(Guid customerId) 
    {
        Query.Where(o => o.CustomerId == customerId);
        Query.Include(o => o.OrderItems);
        Query.Select(OrderMappings.ToDto);
    }
}