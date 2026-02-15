namespace SharedKernel.IntegrationEvents.Payments;
public record PaymentRefundedIntegrationEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    DateTime OccurredOn);