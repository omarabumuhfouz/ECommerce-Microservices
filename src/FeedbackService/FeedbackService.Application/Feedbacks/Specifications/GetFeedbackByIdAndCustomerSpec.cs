using Ardalis.Specification;

namespace FeedbackService.Application.Feedbacks.Specifications;

public class GetFeedbackByIdAndCustomerSpec : Specification<Feedback>
{
    public GetFeedbackByIdAndCustomerSpec(Guid feedbackId, Guid customerId, bool withTracking = false) 
    {
        Query.Where(f => f.Id == feedbackId && f.CustomerId == customerId);

        if (withTracking) Query.AsTracking();
    }
}