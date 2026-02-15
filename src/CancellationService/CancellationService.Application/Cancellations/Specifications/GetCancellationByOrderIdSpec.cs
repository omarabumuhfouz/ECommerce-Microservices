using Ardalis.Specification;

namespace CancellationService.Application.Cancellations.Specifications;
public class GetCancellationByOrderIdSpec : Specification<Cancellation>
{
    public GetCancellationByOrderIdSpec(Guid orderId, bool withTracking = false)
    {
        Query.Where(c => c.OrderId == orderId);

        if (withTracking) Query.AsTracking();
    }
}