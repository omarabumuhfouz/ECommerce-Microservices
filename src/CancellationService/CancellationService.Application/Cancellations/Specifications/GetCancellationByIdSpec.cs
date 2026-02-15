using Ardalis.Specification;

namespace CancellationService.Application.Cancellations.Specifications;
public class GetCancellationByIdSpec : Specification<Cancellation>
{
    public GetCancellationByIdSpec(Guid cancellationId, bool withTracking = false)
    {
        Query.Where(c => c.Id == cancellationId);

        if (withTracking) Query.AsTracking();
    }
}