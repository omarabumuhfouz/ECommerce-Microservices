namespace OrderService.Api.Contracts.Orders;

public record CreateItemDto(Guid ProductId, string ProductName, int Quantity);