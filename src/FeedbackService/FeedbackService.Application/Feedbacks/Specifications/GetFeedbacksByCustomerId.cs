using Ardalis.Specification;

namespace FeedbackService.Application.Feedbacks.Specifications;

public class GetFeedbacksByCustomerId : Specification<Feedback>
{
    public GetFeedbacksByCustomerId(Guid customerId, bool withTracking = false)
    {
        Query.Where(f => f.CustomerId == customerId);

        if (withTracking) Query.AsTracking();
    }

}