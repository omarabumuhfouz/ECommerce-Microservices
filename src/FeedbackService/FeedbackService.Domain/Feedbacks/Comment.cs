using FeedbackService.Domain.Errors;

namespace FeedbackService.Domain.Feedbacks;

public sealed record Comment
{
    public string Value { get; }

    private Comment(string value) => Value = value;

    public static Result<Comment> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return DomainErrors.Feedback.CommentEmpty;

        if (value.Length > FeedbackConstants.MaxCommentLength)
            return DomainErrors.Feedback.CommentTooLong(FeedbackConstants.MaxCommentLength);

        return new Comment(value);
    }

    public static implicit operator string(Comment comment) => comment.Value;
}
