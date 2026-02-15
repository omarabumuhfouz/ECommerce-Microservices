namespace ShoppingCartService.Application.Carts.Commands.EditCartItem;

public class EditCartItemCommandValidator : AbstractValidator<EditCartItemCommand>
{
    public EditCartItemCommandValidator()
    {
        RuleFor(c => c.CustomerId)
            .ValidateCustomerId();

        RuleFor(c => c.CartItemId)
            .ValidateCartItemId();

        RuleFor(c => c.Quantity)
            .ValidateQuantity();
        
    }
}