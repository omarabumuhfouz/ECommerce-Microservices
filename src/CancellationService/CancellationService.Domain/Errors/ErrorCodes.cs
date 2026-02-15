namespace CancellationService.Domain.Errors;

public static class ErrorCodes
{
    public static class Cancellation
    {
        public const string NotFound = "Cancellation.NotFound";
        public const string AlreadyProcessed = "Cancellation.AlreadyProcessed";
        public const string NotPending = "Cancellation.NotPending";
        public const string NotProcessed = "Cancellation.NotProcessed";
        public const string InvalidProcessedBy = "Cancellation.InvalidProcessedBy";
        public const string InvalidProcessedDate = "Cancellation.InvalidProcessedDate";
        public const string InvalidStatusChange = "Cancellation.InvalidStatusChange";
        public const string IsLocked = "Cancellation.IsLocked";

        // Validations
        public const string IdRequired = "Cancellation.IdRequired";
        public const string AdminIdRequired = "Cancellation.AdminIdRequired";
        public const string NotEligibleForRefund = "Cancellation.NotEligibleForRefund";
        public const string NotFoundForOrder = "Cancellation.NotFoundForOrder";
    }

    public static class Money
    {
        public const string Negative = "Money.Negative";
    }

    public static class Reason
    {
        public const string Empty = "Reason.Empty";
        public const string TooLong = "Reason.TooLong";
    }

    public static class Remarks
    {
        public const string TooLong = "Remarks.TooLong";
    }

    public static class Payment
    {
        public const string RefundFailed = "Payment.RefundFailed";
    }

    public static class Validation
    {
        public const string OrderIdRequired = "Validation.OrderIdRequired";
        public const string CustomerIdRequired = "Validation.CustomerIdRequired";
    }

    public static class Order
    {
        public const string NotFound = "Order.NotFound";
        public const string IdRequired = "Order.IdRequired";
    }
}