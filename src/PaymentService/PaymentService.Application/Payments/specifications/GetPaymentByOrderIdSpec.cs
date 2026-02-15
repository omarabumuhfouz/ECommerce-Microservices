using SharedKernel.Specifications;

namespace PaymentService.Application.Payments.specifications;

public class GetPaymentByOrderIdSpec : Specification<Payment>
{
    public GetPaymentByOrderIdSpec(Guid orderId, bool withTracking = false) : base(p => p.OrderId == orderId)
    {
        if (withTracking) EnableTracking();
    }
}