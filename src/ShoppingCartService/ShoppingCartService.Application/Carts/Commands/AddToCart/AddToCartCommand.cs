namespace ShoppingCartService.Application.Carts.Commands.AddToCart;

public record AddToCartCommand(
    Guid CustomerId,
    Guid ProductId,
    int Quantity
) : ICommand<CartDto>;