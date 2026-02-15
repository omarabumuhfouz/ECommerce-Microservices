namespace SharedKernel.IntegrationEvents.Refunds;
public record RefundFailedIntegrationEvent(
    Guid RefundId,
    Guid OrderId,
    string FailureReason,
    DateTime OccurredOn
);