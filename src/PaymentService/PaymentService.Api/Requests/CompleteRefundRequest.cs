namespace PaymentService.Api.Requests;

public record CompleteRefundRequest(string TransactionId, Guid ApprovedBy);