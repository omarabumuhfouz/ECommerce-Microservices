namespace SharedKernel.IntegrationEvents.Payments;
public record PaymentFailedIntegrationEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    // string ErrorCode,
    // string ErrorMessage,
    DateTime OccurredOn);