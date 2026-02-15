using Contracts.Order;

namespace OrderService.Application.Orders.DTOs;

public class OrderDto
{
    // Basic Info
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;

    // Customer Info
    public Guid CustomerId { get; set; }
    
    // Addresses (Often flattened or kept as IDs depending on UI needs)
    public Guid BillingAddressId { get; set; }
    public Guid ShippingAddressId { get; set; }

    // Financial Totals (Calculated from your domain logic)
    public decimal ShippingCost { get; set; }
    public string Currency { get; set; } = "USD"; // Useful if Money has a currency
    public decimal TotalBaseAmount { get; set; }
    public decimal TotalDiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }

    // Nested Items
    public List<OrderItemDto> Items { get; set; } = new();

    public OrderModel ToGrpcModel()
    {
        return new OrderModel
        {
            OrderId = Id.ToString(),
            Status = Status.ToString(),
            TotalAmount = TotalAmount.ToString(),
        };
    }
}
