namespace SharedKernel.IntegrationEvents.Payments;

public record CodPaymentCompletedIntegrationEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    DateTime OccurredOn
);