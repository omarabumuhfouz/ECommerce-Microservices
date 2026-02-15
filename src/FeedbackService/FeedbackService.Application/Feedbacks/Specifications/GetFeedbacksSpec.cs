using Ardalis.Specification;

namespace FeedbackService.Application.Feedbacks.Specifications;

public class GetFeedbacksSpec : Specification<Feedback, FeedbackDto>
{
    public GetFeedbacksSpec(bool withTracking = false) : base()
    {
        if (withTracking) Query.AsTracking();

        Query.Select(f => new FeedbackDto
        {
            Id = f.Id,
            CustomerId = f.CustomerId,
            ProductId = f.ProductId,
            CustomerName = f.CustomerName,
            ProductName = f.ProductName,
            Rating = f.Rating,
            Comment = f.Comment,
            CreatedAt = f.CreatedAt,
        });
    }
}