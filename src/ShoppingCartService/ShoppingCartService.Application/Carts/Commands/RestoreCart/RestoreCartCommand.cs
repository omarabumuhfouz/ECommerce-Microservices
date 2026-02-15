namespace ShoppingCartService.Application.Carts.Commands.RestoreCart;

public record RestoreCartCommand(Guid CustomerId) : ICommand<Unit>;