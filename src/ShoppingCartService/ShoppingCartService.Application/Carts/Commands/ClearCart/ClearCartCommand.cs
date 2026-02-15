namespace ShoppingCartService.Application.Carts.Commands.ClearCart;

public record ClearCartCommand(Guid CustomerId) : ICommand<Unit>;
