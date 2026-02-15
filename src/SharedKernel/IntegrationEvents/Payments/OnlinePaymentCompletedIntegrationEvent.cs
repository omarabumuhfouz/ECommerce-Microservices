namespace SharedKernel.IntegrationEvents.Payments;

public record OnlinePaymentCompletedIntegrationEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    DateTime OccurredOn
);
