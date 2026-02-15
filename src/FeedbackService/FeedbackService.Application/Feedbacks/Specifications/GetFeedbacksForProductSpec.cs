using Ardalis.Specification;

namespace FeedbackService.Application.Feedbacks.Specifications;
public class GetFeedbacksForProductSpec : Specification<Feedback>
{
    public GetFeedbacksForProductSpec(Guid productId, bool withTracking = false)
    {
        if (withTracking) Query.AsTracking();

        Query.Where(f => f.ProductId == productId);

        // Query.Select(f => new FeedbackDto
        // {
        //     Id = f.Id,
        //     ProductId = f.ProductId,
        //     ProductName = f.ProductName,
        //     CustomerId = f.CustomerId,
        //     CustomerName = f.CustomerName,
        //     Rating = f.Rating,
        //     Comment = f.Comment,
        //     CreatedAt = f.CreatedAt
           
        // });
    }
}