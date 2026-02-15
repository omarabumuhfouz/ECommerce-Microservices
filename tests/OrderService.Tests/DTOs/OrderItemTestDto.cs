namespace OrderService.Tests.DTOs;

// OrderItemTestDto.cs
public class OrderItemTestDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercentage { get; set; }

    public OrderItem ToDomain()
    {
        return  OrderItemBuilder.Empty()
                       .WithId(Id)
                       .WithOrderId(OrderId)
                       .WithProductId(ProductId)
                       .WithQuantity(Quantity)
                       .WithUnitPrice(UnitPrice)
                       .WithDiscountPercentage(DiscountPercentage)
                       .Build();
    }
}
