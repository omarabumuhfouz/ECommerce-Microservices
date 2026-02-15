namespace FeedbackService.Domain.Exceptions
{
    public class ProductNotEligibleForFeedback : Exception
    {
        public ProductNotEligibleForFeedback() : base("Invalid OrderItemId. Customer must have purchased the product.")
        {

        }
    }
}
