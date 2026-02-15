using Contracts.Order;
using PaymentService.Application.Common.Enums;

namespace PaymentService.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }

    public static OrderDto FromGrpcModel(OrderModel model)
    {
        return new OrderDto
        {
            Id = Guid.Parse(model.OrderId),
            Status = Enum.Parse<OrderStatus>(model.Status, ignoreCase: true),
            TotalAmount = Convert.ToDecimal(model.TotalAmount)
        };
    }
}
