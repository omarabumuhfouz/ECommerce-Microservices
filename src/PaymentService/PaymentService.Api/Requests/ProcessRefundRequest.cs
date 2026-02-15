namespace PaymentService.Api.Requests;

public record ProcessRefundRequest(
    Guid OrderId,
    Guid CancellationId,
    Guid ApprovedByUserId,
    decimal Amount,
    string Remarks);