namespace SharedKernel.IntegrationEvents.Payments;
public record PaymentPendingIntegrationEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    DateTime OccurredOn);