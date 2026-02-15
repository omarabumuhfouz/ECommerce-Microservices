using PaymentService.Application.Common.Extensions;

namespace PaymentService.Application.Payments.Queries.GetPaymentByOrderId;
public class GetPaymentByOrderIdQueryValidator : AbstractValidator<GetPaymentByOrderIdQuery>
{
    public GetPaymentByOrderIdQueryValidator()
    {
        RuleFor(c => c.OrderId)
            .ValidateOrderId();
    }
}