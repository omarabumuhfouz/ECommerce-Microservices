using CustomerService.Application.Extensions;

namespace OrderService.Application.Orders.Queries.IsProductEligibleForFeedback;

public class IsProductEligibleForFeedbackQueryValidator : AbstractValidator<IsProductEligibleForFeedbackQuery>
{
    public IsProductEligibleForFeedbackQueryValidator()
    {
        RuleFor(x => x.CustomerId)
        .ValidateCustomerId();

        RuleFor(x => x.ProductId)
        .ValidateProductId();
    }
}