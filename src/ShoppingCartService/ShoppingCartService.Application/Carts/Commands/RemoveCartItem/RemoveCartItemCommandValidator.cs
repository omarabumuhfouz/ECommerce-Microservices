namespace ShoppingCartService.Application.Carts.Commands.RemoveCartItem;
public class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(c => c.CustomerId)
            .ValidateCustomerId();

        RuleFor(c => c.CartItemId)
            .ValidateCartItemId();
    }
}