using System.Linq.Expressions;
using Ardalis.Specification;
using OrderService.Application.Orders.Mappings;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.Enums;

namespace OrderService.Application.Orders.Specifications;

public class GetOrdersSpec : Specification<Order, OrderDto>
{
    public GetOrdersSpec(
        string? searchTerm, 
        string? status, 
        Guid? customerId,
        string sortBy, 
        bool isAscending, 
        int pageNumber, 
        int pageSize)
    {
        OrderStatus? statusEnum = Enum.TryParse<OrderStatus>(status, true, out var parsedStatus) 
            ? parsedStatus 
            : null;

        Query.Where(o => o.OrderNumber.Value.Contains(searchTerm!), !string.IsNullOrWhiteSpace(searchTerm))
             .Where(o => o.OrderStatus == statusEnum, statusEnum.HasValue)
             .Where(o => o.CustomerId == customerId, customerId.HasValue);

        Expression<Func<Order, object>> keySelector = sortBy.ToLower() switch
        {
            "status" => p => p.OrderStatus,
            "number" => p => p.OrderNumber.Value, 
            "total" => p => p.TotalAmount,
            _ => p => p.OrderDate
        };

        if (isAscending) Query.OrderBy(keySelector!);
        else Query.OrderByDescending(keySelector!);

        Query.Skip((pageNumber - 1) * pageSize)
             .Take(pageSize);

        Query.Select(OrderMappings.ToDto);
    }
}