namespace FeedbackService.Api.Contracts;

public record SubmitFeedbackRequest(
    Guid CustomerId,
    Guid ProductId,
    string ProductName,
    string CustomerName,
    int Rating,
    string Comment
);
