namespace SharedKernel.IntegrationEvents.Cancellations;

public record CancellationRejectedIntegrationEvent(
    Guid CancellationId,
    Guid OrderId,
    string Remarks,
    DateTime OccurredOn
);