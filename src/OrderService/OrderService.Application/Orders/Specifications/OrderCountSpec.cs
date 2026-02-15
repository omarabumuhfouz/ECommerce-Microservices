using Ardalis.Specification;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.Enums;

namespace OrderService.Application.Orders.Specifications;
public class OrderCountSpec : Specification<Order>
{
    public OrderCountSpec(string? searchTerm, string? status, Guid? customerId)
    {
OrderStatus? statusEnum = Enum.TryParse<OrderStatus>(status, true, out var parsedStatus) 
            ? parsedStatus 
            : null;

        Query.Where(o => o.OrderNumber.Value.Contains(searchTerm!), !string.IsNullOrWhiteSpace(searchTerm))
             .Where(o => o.OrderStatus == statusEnum, statusEnum.HasValue)
             .Where(o => o.CustomerId == customerId, customerId.HasValue);
    }
}