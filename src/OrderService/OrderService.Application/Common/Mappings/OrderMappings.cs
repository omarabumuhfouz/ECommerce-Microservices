using System.Linq.Expressions;
using OrderService.Domain.Orders;

namespace OrderService.Application.Orders.Mappings;

public static class OrderMappings
{
    public static Expression<Func<Order, OrderDto>> ToDto => order => new OrderDto
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        OrderNumber = order.OrderNumber.Value,
        OrderDate = order.OrderDate,
        Status = order.OrderStatus.ToString(),

        BillingAddressId = order.BillingAddressId,
        ShippingAddressId = order.ShippingAddressId,

        ShippingCost = order.ShippingCost.Value,
        TotalBaseAmount = order.TotalBaseAmount.Value,
        TotalDiscountAmount = order.TotalDiscountAmount.Value,
        TotalAmount = order.TotalAmount.Value,

        Items = order.OrderItems.Select(item => new OrderItemDto
        {
            Id = item.Id,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice.Value,
            Discount = item.Discount.Value
        }).ToList()
    };
}