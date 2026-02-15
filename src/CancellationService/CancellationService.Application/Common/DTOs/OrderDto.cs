using CancellationService.Application.Enums;
using Contracts.Order;

namespace CancellationService.Application.Cancellations.DTOs;

public record OrderDto(Guid Id, OrderStatus Status, decimal TotalAmount)
{
    // Secondary constructor must call the primary constructor using ': this'
    public static OrderDto FromModel(OrderModel model) => new(
        Guid.Parse(model.OrderId), 
        Enum.Parse<OrderStatus>(model.Status), 
        Convert.ToDecimal(model.TotalAmount));
}
