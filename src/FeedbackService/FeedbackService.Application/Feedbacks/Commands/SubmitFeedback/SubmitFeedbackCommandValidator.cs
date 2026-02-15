using FeedbackService.Application.Common.Extensions;

namespace FeedbackService.Application.Feedbacks.Commands.SubmitFeedback;

public class SubmitFeedbackCommandValidator : AbstractValidator<SubmitFeedbackCommand>
{
    public SubmitFeedbackCommandValidator()
    {
        RuleFor(f => f.CustomerId)
            .ValidateCustomerId();

        RuleFor(f => f.ProductId)
            .ValidateProductId();

        RuleFor(f => f.Rating)
            .ValidateRating();

        RuleFor(f => f.Comment)
            .ValidateComment();
    }
}