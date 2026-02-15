namespace PaymentService.Domain.Errors;

public static class ErrorCodes
{
    public static class Payment
    {
        public const string NotFound = "Payment.NotFound";
        public const string InvalidAmount = "Payment.InvalidAmount";
        public const string InvalidPaymentMethod = "Payment.InvalidPaymentMethod";
        public const string InvalidStatusChange = "Payment.InvalidStatusChange";
        public const string CodNotEligibleForCompletion = "Payment.CodNotEligibleForCompletion";
        public const string PaymentIdRequired = "Payment.IdRequired";
        public const string OrderIdRequired = "Payment.OrderIdRequired";
        public const string InvalidStatus = "Payment.InvalidStatus";
        public const string AmountMismatchCode = "Payment.AmountMismatch";
        public const string NotCashOnDeliveryCode = "Payment.NotCashOnDelivery";
        public const string OrderFetchFailed = "Payment.OrderFetchFailed";
        public const string NonRetriableState = "Payment.NonRetriableState";
        public const string NotEditable = "Payment.NotEditable";
        public const string CannotRefundUncompletedPayment = "Payment.CannotRefundUncompletedPayment";
        public const string FinalState = "Payment.FinalState";
        public const string RefundExceedsOriginalAmount = "Payment.RefundExceedsOriginalAmount";
    }

    public static class Order
    {
        public const string InvalidStatusChangeCode = "Order.InvalidStatusChange";
        public const string NotFound = "Order.NotFound";
        public const string GrpcError = "OrderService.GrpcError";
    }

    public static class Refund
    {
        public const string NotFound = "Refund.NotFound";
        public const string RefundAlreadyExists = "Refund.RefundAlreadyExists";
        public const string InvalidAmount = "Refund.InvalidAmount";
        public const string ReasonTooLong = "Refund.ReasonTooLong";
        public const string TransactionIdTooLong = "Refund.TransactionIdTooLong";
        public const string InvalidCompletionDate = "Refund.InvalidCompletionDate";
        public const string AlreadyCompleted = "Refund.AlreadyCompleted";
        public const string InvalidTransactionId = "Refund.InvalidTransactionId";
        public const string InvalidReason = "Refund.InvalidReason";

    }

}