using FeedbackService.Application.Common.Extensions;

namespace FeedbackService.Application.Feedbacks.Commands.DeleteFeedback;

public class DeleteFeedbackCommandValidator : AbstractValidator<DeleteFeedbackCommand>
{
    public DeleteFeedbackCommandValidator()
    {
        RuleFor(f => f.CustomerId)
            .ValidateCustomerId();

        RuleFor(f => f.FeedbackId)
            .ValidateFeedbackId();
    }
}