using FeedbackService.Domain.Errors;
using SharedKernel.Shared;

namespace FeedbackService.Domain.Feedbacks;

public sealed record Rating
{
    public int Value { get; }

    private Rating(int value) => Value = value;

    public static Result<Rating> Create(int value)
    {
        if (value < 1 || value > 5) return DomainErrors.Feedback.RatingInvalid;

        return new Rating(value);
    }

    
    // Implicit conversion allows you to treat Rating as int easily
    public static implicit operator int(Rating rating) => rating.Value;
}
