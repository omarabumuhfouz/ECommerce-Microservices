
namespace FeedbackService.Application.Common.Extensions;

internal static class FeedbackValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> ValidateCustomerId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Customer.IdRequired);
    }

    public static IRuleBuilderOptions<T, Guid> ValidateFeedbackId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Feedback.IdRequired);
    }

    public static IRuleBuilderOptions<T, Guid> ValidateProductId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Product.IdRequired);
    }

    public static IRuleBuilderOptions<T, int> ValidateRating<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .GreaterThanOrEqualTo(FeedbackConstants.MinRating)
            .LessThanOrEqualTo(FeedbackConstants.MaxRating)
            .WithErrorCode(ErrorCodes.Feedback.RatingInvalid);
    }

    public static IRuleBuilderOptions<T, string> ValidateComment<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .MaximumLength(FeedbackConstants.MaxCommentLength)
            .WithErrorCode(ErrorCodes.Feedback.CommentInvalid);
    }
        
}
