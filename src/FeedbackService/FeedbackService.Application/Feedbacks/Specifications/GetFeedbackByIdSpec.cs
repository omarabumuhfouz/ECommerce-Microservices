using Ardalis.Specification;

namespace FeedbackService.Application.Feedbacks.Specifications;
public class GetFeedbackByIdSpec : Specification<Feedback>
{
    public GetFeedbackByIdSpec(Guid feedbackId, bool withTracking = false)
    {
        if (withTracking) Query.AsTracking();

        Query.Where(f => f.Id == feedbackId);
    }
}