namespace FeedbackService.Application.Feedbacks.Commands.SubmitFeedback;

public record SubmitFeedbackCommand(
    Guid CustomerId,
    Guid ProductId,
    string CustomerName,
    string ProductName,
    int Rating,
    string Comment

) : ICommand<FeedbackDto>;