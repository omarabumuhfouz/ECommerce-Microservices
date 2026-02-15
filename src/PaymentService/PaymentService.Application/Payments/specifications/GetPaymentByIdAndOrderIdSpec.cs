using SharedKernel.Specifications;

namespace PaymentService.Application.Payments.specifications;
public class GetPaymentByIdAndOrderIdSpec : Specification<Payment>
{
    public GetPaymentByIdAndOrderIdSpec(Guid paymentId, Guid orderId, bool withTracking = false)
        : base(p => p.Id == paymentId && p.OrderId == orderId)
    {
        if (withTracking) EnableTracking();

        
    }
}