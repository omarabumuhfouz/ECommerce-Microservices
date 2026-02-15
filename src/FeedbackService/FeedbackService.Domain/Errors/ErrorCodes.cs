namespace FeedbackService.Domain.Errors;

public static class ErrorCodes
{
    public static class Feedback
    {
        public const string NoFeedbacksFound = "Feedback.NoFeedbacksFound";
        public const string NotFound = "Feedback.NotFound";
        public const string RatingInvalid = "Feedback.RatingInvalid";
        public const string CommentInvalid = "Feedback.CommentInvalid";
        public const string CommentEmpty = "Feedback.CommentEmpty";
        public const string CommentTooLong = "Feedback.CommentTooLong";
        public const string IdRequired = "Feedback.IdRequired";
        public const string NotFoundForProduct = "Feedback.NotFoundForProduct";
        public const string ProductNotPurchased = "Feedback.ProductNotPurchased";
        public const string AlreadyReviewed = "Feedback.AlreadyReviewed";
    }


    public static class Customer
    {
        public const string IdRequired = "Customer.IdRequired";
        public const string NameRequired = "Customer.NameRequired";
    }

    public static class Product
    {
        public const string IdRequired = "Product.IdRequired";
        public const string NameRequired = "Product.NameRequired";
    }

}