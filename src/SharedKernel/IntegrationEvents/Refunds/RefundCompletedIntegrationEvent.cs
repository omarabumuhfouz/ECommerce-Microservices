namespace SharedKernel.IntegrationEvents.Refunds;

public record RefundCompletedIntegrationEvent(
    Guid RefundId,
    Guid OrderId,
    decimal Amount,
    DateTime OccurredOn
);

