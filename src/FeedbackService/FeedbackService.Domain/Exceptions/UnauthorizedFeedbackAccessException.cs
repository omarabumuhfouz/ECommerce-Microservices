/// <summary>
/// The exception that is thrown when a user attempts to modify or delete feedback they do not own.
/// </summary>
/// <remarks>
/// This exception is typically thrown during feedback operations (delete/update) when the system detects
/// that the requesting user does not match the feedback's owner.
/// </remarks>
public class UnauthorizedFeedbackAccessException : Exception
{
    /// <summary>
    /// Gets the ID of the feedback that the user attempted to modify.
    /// </summary>
    /// <value>The feedback ID as stored in the database.</value>
    public int FeedbackId { get; }

    /// <summary>
    /// Gets the ID of the user who attempted the unauthorized action.
    /// </summary>
    /// <value>The user ID extracted from the request context.</value>
    public int AttemptedUserId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedFeedbackAccessException"/> class.
    /// </summary>
    /// <param name="feedbackId">The ID of the target feedback.</param>
    /// <param name="attemptedUserId">The ID of the user attempting the action.</param>
    /// <example>
    /// <code>
    /// if (feedback.OwnerId != currentUserId)
    /// {
    ///     throw new UnauthorizedFeedbackAccessException(feedback.Id, currentUserId);
    /// }
    /// </code>
    /// </example>
    public UnauthorizedFeedbackAccessException(int feedbackId, int attemptedUserId)
        : base($"User {attemptedUserId} is not authorized to modify feedback {feedbackId}")
    {
        FeedbackId = feedbackId;
        AttemptedUserId = attemptedUserId;
    }
}