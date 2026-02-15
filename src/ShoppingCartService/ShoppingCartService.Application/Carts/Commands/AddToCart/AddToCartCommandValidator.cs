namespace ShoppingCartService.Application.Carts.Commands.AddToCart;

public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
{
    public AddToCartCommandValidator()
    {
        RuleFor(c => c.CustomerId)
            .ValidateCustomerId();

        RuleFor(c => c.ProductId)
            .ValidateProductId();

        RuleFor(c => c.Quantity)
            .ValidateQuantity();
    }

}