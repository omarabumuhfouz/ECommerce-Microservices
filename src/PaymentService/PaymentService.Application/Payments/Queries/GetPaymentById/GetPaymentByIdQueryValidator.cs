using PaymentService.Application.Common.Extensions;

namespace PaymentService.Application.Payments.Queries.GetPaymentById;
public class GetPaymentByIdQueryValidator : AbstractValidator<GetPaymentByIdQuery>
{
    public GetPaymentByIdQueryValidator()
    {
        RuleFor(c => c.PaymentId)
            .ValidatePaymentId();
        
    }
}