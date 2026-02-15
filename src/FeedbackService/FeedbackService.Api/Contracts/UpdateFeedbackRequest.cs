namespace FeedbackService.Api.Contracts;
public record UpdateFeedbackRequest(
    Guid CustomerId,
    int Rating,
    string Comment
);