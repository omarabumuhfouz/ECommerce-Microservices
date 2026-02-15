using OrderService.Domain.Constants;

namespace CustomerService.Application.Extensions;

internal static class OrderValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> ValidateUserId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Customer.UserIdRequired);
    }

    public static IRuleBuilderOptions<T, Guid> ValidateCustomerId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Customer.IdRequired);
    }

    public static IRuleBuilderOptions<T, Guid> ValidateOrderId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Order.IdRequired);
    }

    public static IRuleBuilderOptions<T, Guid> ValidatePaymentId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Payment.IdRequired);
    }
        
    public static IRuleBuilderOptions<T, Guid> ValidateProductId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Product.IdRequired);
    }
        
}
