namespace FeedbackService.Application.Feedbacks.Queries.GetFeedbackForProduct;

public record GetFeedbackForProductQuery(Guid ProductId) : IQuery<ProductFeedbackDto>;

