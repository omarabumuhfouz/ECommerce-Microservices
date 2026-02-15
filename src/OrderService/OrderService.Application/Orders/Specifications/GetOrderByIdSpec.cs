using Ardalis.Specification;
using OrderService.Domain.Orders;

namespace OrderService.Application.Orders.Specifications;

public class GetOrderByIdSpec : Specification<Order>
{
    public GetOrderByIdSpec(Guid orderId, bool withTracking = false)
    {
        Query.Where(o => o.Id == orderId);
        Query.Include(o => o.OrderItems);

        if (withTracking) Query.AsTracking();

    }
}
