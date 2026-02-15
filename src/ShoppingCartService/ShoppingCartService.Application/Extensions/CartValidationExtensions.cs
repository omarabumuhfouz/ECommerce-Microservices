using ShoppingCartService.Domain.Constants;

namespace ShoppingCartService.Application.Extensions;

internal static class CartValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> ValidateCustomerId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Cart.CustomerIdRequired);
    }

    public static IRuleBuilderOptions<T, Guid> ValidateProductId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.CartItem.InvalidProductId);
    }
    
    public static IRuleBuilderOptions<T, Guid> ValidateCartItemId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.CartItem.IdRequired);
    }
    
    public static IRuleBuilderOptions<T, int> ValidateQuantity<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Quantity.Required)
            .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCodes.Quantity.InvalidAmount);
    }
}
