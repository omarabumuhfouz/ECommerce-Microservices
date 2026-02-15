namespace OrderService.Application.Orders.Queries.GetOrdersCountByCustomer;

public record GetOrdersCountsByCustomerIdQuery(Guid CustomerId) : IQuery<int>;