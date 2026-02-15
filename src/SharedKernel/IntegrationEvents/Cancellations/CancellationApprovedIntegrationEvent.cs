namespace SharedKernel.IntegrationEvents.Cancellations;

public record CancellationApprovedIntegrationEvent(
    Guid CancellationId,
    Guid OrderId,
    decimal RefundAmount,
    string Remarks,
    Guid ApprovedBy,
DateTime OccurredOn
);