namespace ShoppingCartService.Application.Carts.Commands.CheckoutCart;

public record CheckoutCartCommand(Guid RequestId, Guid CustomerId) : IIdempotentCommand<Unit>{
    public Guid Id => RequestId;
}