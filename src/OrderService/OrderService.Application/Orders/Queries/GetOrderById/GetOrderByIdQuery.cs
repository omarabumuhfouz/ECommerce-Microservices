namespace OrderService.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderDto>;
