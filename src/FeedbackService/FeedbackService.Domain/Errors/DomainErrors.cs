namespace FeedbackService.Domain.Errors;

public static class DomainErrors
{
    public static class Feedback
    {
        public static Error NotFound(Guid feedbackId, Guid customerId) => Error.NotFound(
                    ErrorCodes.Feedback.NotFound,
                    $"Feedback with Id '{feedbackId}' was not found for Customer '{customerId}'."
                );

        public static Error NotFoundForProduct(Guid productId) => Error.NotFound(
                    ErrorCodes.Feedback.NotFoundForProduct,
                    $"Feedbacks for Product with Id '{productId}' was not found."
                );

        public static Error NoFeedbacksFound(Guid productId) => Error.NotFound(
              ErrorCodes.Feedback.NoFeedbacksFound,
              $"No feedbacks were found for Product '{productId}'."
          );

        public static Error RatingInvalid => Error.Validation(
            ErrorCodes.Feedback.RatingInvalid,
            "Rating must be between 1 and 5."
        );

        public static Error CommentEmpty => Error.Validation(
            ErrorCodes.Feedback.CommentEmpty,
            "Comment cannot be empty or whitespace."
        );

        public static Error CommentTooLong(int maxLength) => Error.Validation(
            ErrorCodes.Feedback.CommentTooLong,
            $"Comment cannot exceed {maxLength} characters."
    );

        public static Error ProductNotPurchased => Error.Conflict(
            ErrorCodes.Feedback.ProductNotPurchased, 
            "You cannot leave feedback for a product you have not purchased.");

        public static Error AlreadyReviewed => Error.Conflict(
            ErrorCodes.Feedback.AlreadyReviewed, 
            "You have already submitted feedback for this product.");

    }

    public static class Customer
    {
        public static Error NameRequired => Error.Validation(
            ErrorCodes.Customer.NameRequired,
            "Customer name is required."
        );

    }

    public static class Product
    {
public static Error NameRequired => Error.Validation(
            ErrorCodes.Product.NameRequired,
            "Product name is required."
        );


    }


}