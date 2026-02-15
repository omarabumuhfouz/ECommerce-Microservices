namespace OrderService.Domain.Orders.Events;

public record OrderEventItem(Guid ProductId, int Quantity);
