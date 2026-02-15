namespace FeedbackService.Application.Feedbacks.Commands.DeleteFeedback;

public record DeleteFeedbackCommand(Guid FeedbackId, Guid CustomerId) : ICommand<Unit>;

