namespace ShoppingCartService.Application.Carts.Commands.EditCartItem;

public record EditCartItemCommand(
    Guid CustomerId,
    Guid CartItemId,
    int Quantity
) : ICommand<CartDto>;
