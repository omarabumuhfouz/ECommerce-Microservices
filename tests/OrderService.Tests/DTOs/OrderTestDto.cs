using OrderService.Domain.Enums;

namespace OrderService.Tests.DTOs;

public class OrderTestDto
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public Guid CustomerId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public Guid BillingAddressId { get; set; }
    public Guid ShippingAddressId { get; set; }
    public decimal ShippingCost { get; set; }
    public string OrderStatus { get; set; } = string.Empty; // string for JSON parsing
    public List<OrderItemTestDto> OrderItems { get; set; } = new();

    public Order ToDomain()
    {
        var items = OrderItems.Select(dto => dto.ToDomain()).ToList();
        var parsedStatus = Enum.Parse<OrderStatus>(OrderStatus, ignoreCase: true);

        return new Order(
            Id,
            CustomerId,
            PaymentId,
            OrderNumber,
            OrderDate,
            BillingAddressId,
            ShippingAddressId,
            ShippingCost,
            parsedStatus,
            items
        );
    }
}
