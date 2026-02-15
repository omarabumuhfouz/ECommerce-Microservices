namespace FeedbackService.Domain.Exceptions
{
    public class DuplicateFeedbackException : Exception
    {
        public DuplicateFeedbackException() : base("Feedback for this product and order item already exists.")
        {

        }
    }
}
