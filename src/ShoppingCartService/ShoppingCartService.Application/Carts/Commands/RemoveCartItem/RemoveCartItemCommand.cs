namespace ShoppingCartService.Application.Carts.Commands.RemoveCartItem;

public record RemoveCartItemCommand(
    Guid CustomerId,
    Guid CartItemId
) : ICommand<CartDto>;
