namespace ShoppingCartService.Application.Carts.Commands.RestoreCart;

public class RestoreCartCommandValidator : AbstractValidator<RestoreCartCommand>
{
    public RestoreCartCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .ValidateCustomerId();
    }
}