namespace ShoppingCartService.Application.Carts.Commands.ClearCart;

public class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartCommandValidator()
    {
        RuleFor(c => c.CustomerId)
            .ValidateCustomerId();
    }
}