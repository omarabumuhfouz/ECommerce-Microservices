namespace PaymentService.Api.Requests;

public sealed record MarkAsCompletedRequest(string? TransactionId);
