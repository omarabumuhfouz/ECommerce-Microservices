namespace ShoppingCartService.Application.Carts.Queries.GetCartByCustomer;

public record GetCartByCustomerIdQuery(Guid CustomerId) : IQuery<CartDto>;
