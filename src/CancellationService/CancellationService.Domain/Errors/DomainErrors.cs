namespace CancellationService.Domain.Errors;

public static class DomainErrors
{
    public static class Cancellation
    {
        public static Error NotFound(Guid id) => Error.NotFound(
                ErrorCodes.Cancellation.NotFound, // Or use ErrorCodes.Cancellation.NotFound
                $"The cancellation with Id '{id}' was not found."
            );
        public static Error AlreadyProcessed => Error.Conflict(
            ErrorCodes.Cancellation.NotPending, "Only pending cancellations can be rejected.");

        public static Error NotProcessedReopen => Error.Conflict(
            ErrorCodes.Cancellation.NotProcessed, "Only processed cancellations can be reopened.");

        public static Error InvalidProcessedBy => Error.Validation(
            ErrorCodes.Cancellation.InvalidProcessedBy, "Processed By must be a positive integer.");

        public static Error InvalidProcessedDate => Error.Validation(
             ErrorCodes.Cancellation.InvalidProcessedDate, "Processed At date must be a valid DateTime.");

        public static Error InvalidStatusChange(string currentStatus, string targetStatus) => Error.Conflict(
                    ErrorCodes.Cancellation.InvalidStatusChange, // Or use ErrorCodes.Cancellation.InvalidStatusChange constant
                    $"Cannot change cancellation status from '{currentStatus}' to '{targetStatus}'."
                );

    public static Error NotEligible(string status) => Error.Conflict(
        "Cancellation.NotEligible",
        $"Order status Not Eligible, but only 'Processing' orders can be cancelled.");

        public static Error DuplicateCancellation(Guid orderId) => Error.Conflict(
            "Cancellation.Duplicate",
            $"A cancellation request already exists for Order '{orderId}'.");

        public static Error IsLocked => Error.Conflict(
        ErrorCodes.Cancellation.IsLocked,
        "The cancellation request is locked and cannot be modified because it has already been processed.");

        public static Error NotFoundForOrder(Guid orderId) => Error.NotFound(
            ErrorCodes.Cancellation.NotFoundForOrder,
             $"No cancellation found for Order ID '{orderId}'.");

    }

    public static class Money
    {
        public static Error Negative => Error.Validation(
            ErrorCodes.Money.Negative, "Money amount cannot be negative.");
    }

    public static class Reason
    {
        public static Error Empty => Error.Validation(
            ErrorCodes.Reason.Empty, "Reason is required.");

        public static Error TooLong => Error.Validation(
            ErrorCodes.Reason.TooLong, "Reason cannot exceed 500 characters.");
    }

    public static class Remarks
    {
        public static Error TooLong => Error.Validation(
            ErrorCodes.Remarks.TooLong, "Remarks cannot exceed 500 characters.");
    }
    public static class Payment
    {
        public static Error RefundFailed => Error.Failure(
            ErrorCodes.Payment.RefundFailed,
            "The refund process failed at the payment gateway. Please try again later."
        );
    }

    public static class Order
    {
        public static Error NotFound(Guid orderId) => Error.NotFound(
            ErrorCodes.Order.NotFound,
            $"The order with Id '{orderId}' was not found."
        );
    }

}