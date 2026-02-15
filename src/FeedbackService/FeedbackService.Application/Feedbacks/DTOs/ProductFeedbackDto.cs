namespace FeedbackService.Application.Feedbacks.DTOs;

public record ProductFeedbackDto(
    Guid ProductId, 
    string ProductName, 
    double AverageRating, 
    int TotalReviews, 
    List<CustomerFeedbackDto> Feedbacks);

public record CustomerFeedbackDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    int Rating,
    string? Comment,
    DateTime CreatedAt,
    DateTime UpdatedAt
);