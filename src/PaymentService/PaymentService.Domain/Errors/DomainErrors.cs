using SharedKernel.Shared;

namespace PaymentService.Domain.Errors;

public static class DomainErrors
{
    public static class Payment
    {
        public static Error NotFound(Guid id) => Error.NotFound(
            ErrorCodes.Payment.NotFound,
            $"The payment with Id '{id}' was not found.");
        public static Error NotFoundByOrder(Guid orderId) => Error.NotFound(
            ErrorCodes.Payment.NotFound,
            $"The payment for Order Id '{orderId}' was not found.");

        public static Error NotFound(Guid paymentId, Guid orderId) => Error.NotFound(
            ErrorCodes.Payment.NotFound,
            $"The payment with Id '{paymentId}' for Order '{orderId}' was not found.");

        public static Error NotFoundForOrder(Guid orderId) => Error.NotFound(
            ErrorCodes.Payment.NotFound, // You can reuse the existing code, or create a new one like "Payment.OrderNotFound"
            $"The payment for Order Id '{orderId}' was not found.");

        public static readonly Error InvalidAmount = Error.Validation(
            ErrorCodes.Payment.InvalidAmount,
            "Payment amount must be greater than zero.");

        public static readonly Error InvalidPaymentMethod = Error.Validation(
            ErrorCodes.Payment.InvalidPaymentMethod,
            "Payment method cannot be empty or null.");

        public static Error InvalidStatusChange(string current, string target) => Error.Conflict(
            ErrorCodes.Payment.InvalidStatusChange,
            $"Cannot change payment status from '{current}' to '{target}'.");

        public static Error CodNotEligibleForCompletion(string orderStatus) => Error.Conflict(
            ErrorCodes.Payment.CodNotEligibleForCompletion,
            $"Cash on Delivery payments can only be completed when Order Status is 'Shipped' or 'Delivered'. Current status is '{orderStatus}'.");

        public static Error AmountMismatch(decimal expected, decimal actual) => Error.Conflict(
            ErrorCodes.Payment.AmountMismatchCode,
            $"Payment amount '{actual}' does not match the required order amount '{expected}'.");

        public static Error NotCashOnDelivery(string currentMethod) => Error.Conflict(
            ErrorCodes.Payment.NotCashOnDeliveryCode,
            description: $"The operation failed because the payment method is not 'COD'. It is '{currentMethod}'."
        );

        public static Error FetchOrderFailed(Guid orderId) => Error.Failure(
                    "Payment.OrderFetchFailed",
                    $"Failed to retrieve order details for OrderId '{orderId}'."
                );

            public static Error NotEditable(string currentStatus, string operation) => Error.Conflict(
            "Payment.NotEditable",
            $"Cannot perform '{operation}' because the payment is currently '{currentStatus}' and cannot be modified."
        );

        public static Error NonRetriableState(string currentStatus)
        {

            // Customize the message based on the status
            string message = currentStatus switch
            {
                "Completed" => "This order has already been fully paid. No further payment is required.",
                "Processing" => "A payment is currently being processed for this order. Please wait for it to finish.",
                _ => $"Cannot process payment. The current status '{currentStatus}' does not allow retries."
            };

            return Error.Conflict(ErrorCodes.Payment.NonRetriableState, message);
        }

        public static Error CannotRefundUncompletedPayment(string currentStatus) => Error.Conflict(
            ErrorCodes.Payment.CannotRefundUncompletedPayment,
            $"Cannot refund a payment with status '{currentStatus}'. Only completed payments can be refunded.");

        public static Error CannotUpdate =>
             Error.Conflict(
                ErrorCodes.Payment.FinalState,
                $"Cannot update payment because it is in the final state");

        public static Error RefundExceedsOriginalAmount => Error.Conflict(
        ErrorCodes.Payment.RefundExceedsOriginalAmount,
        "The total refund amount cannot exceed the original payment amount.");

        public static Error AutomaticRefundNotSupported => Error.Conflict(
            "Payment.AutomaticRefundNotSupported",
            "This operation is not supported for automatic refund methods. It is intended for manual refunds (e.g., COD) only.");
    }


    public static class Order
    {
        public static Error InvalidStatusChange(string current, string expected) => Error.Conflict(
            ErrorCodes.Order.InvalidStatusChangeCode,
            $"Order cannot be changed to '{expected}' because it is currently '{current}'.");

        public static Error NotFound(Guid id) => Error.NotFound(
            ErrorCodes.Order.NotFound,
            $"The order with Id '{id}' was not found.");

        public static Error GrpcError(string statusCode, string details) => Error.Failure(
            ErrorCodes.Order.GrpcError,
            $"gRPC call failed with status: {statusCode}. Details: {details}");
    }

    public static class Refund
    {
        public static Error NotFound(Guid id) => Error.NotFound(
                ErrorCodes.Refund.NotFound,
                $"The refund with Id '{id}' was not found."
            );

        public static Error RefundAlreadyExists(Guid cancellationId) => Error.Conflict(
        ErrorCodes.Refund.RefundAlreadyExists,
        $"A refund for Cancellation ID '{cancellationId}' already exists."
            );
        public static Error InvalidAmount => Error.Validation(
            ErrorCodes.Refund.InvalidAmount,
            "Refund amount must be greater than zero.");

        public static Error ReasonTooLong => Error.Validation(
            ErrorCodes.Refund.ReasonTooLong,
            "Refund reason cannot exceed 500 characters.");

        public static Error TransactionIdTooLong => Error.Validation(
            ErrorCodes.Refund.TransactionIdTooLong,
            "Transaction ID cannot exceed 100 characters.");

        public static Error InvalidTransactionId => Error.Validation(
            ErrorCodes.Refund.InvalidTransactionId,
            "Transaction ID cannot be empty.");

        public static Error InvalidCompletionDate => Error.Conflict(
            ErrorCodes.Refund.InvalidCompletionDate,
            "Completion date cannot be before the initiated date.");

        public static Error AlreadyCompleted => Error.Conflict(
            ErrorCodes.Refund.AlreadyCompleted,
            "This refund has already been completed and cannot be modified.");

        public static Error InvalidReason => Error.Validation(
                ErrorCodes.Refund.InvalidReason,
                "The refund reason cannot be empty or whitespace.");
    }

    
}