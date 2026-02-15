namespace FeedbackService.Application.Feedbacks.Commands.UpdateFeedback;

public record UpdateFeedbackCommand(
    Guid FeedbackId,
    Guid CustomerId,
    int Rating,
    string Comment

) : ICommand<FeedbackDto>;
