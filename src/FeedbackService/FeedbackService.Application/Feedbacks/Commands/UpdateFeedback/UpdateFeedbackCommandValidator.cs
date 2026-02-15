using System.Data;
using FeedbackService.Application.Common.Extensions;

namespace FeedbackService.Application.Feedbacks.Commands.UpdateFeedback;

public class UpdateFeedbackCommandValidator : AbstractValidator<UpdateFeedbackCommand>
{
    public UpdateFeedbackCommandValidator()
    {
        RuleFor(f => f.CustomerId)
            .ValidateCustomerId();

        RuleFor(f => f.FeedbackId)
            .ValidateFeedbackId();

        RuleFor(f => f.Comment)
            .ValidateComment();

        RuleFor(f => f.Rating)
            .ValidateRating();
        
    }
}