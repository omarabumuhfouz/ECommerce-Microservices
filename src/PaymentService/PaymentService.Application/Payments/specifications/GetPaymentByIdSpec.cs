using SharedKernel.Specifications;

namespace PaymentService.Application.Payments.specifications;
public class GetPaymentByIdSpec : Specification<Payment>
{
    public GetPaymentByIdSpec(Guid paymentId, bool withTracking = false) : base(p => p.Id == paymentId)
    {
        if (withTracking) EnableTracking();
    }


}