namespace SharedKernel.IntegrationEvents.Customers;

public record CustomerCreationFailedEvent(
    Guid UserId,
    string Reason,
    DateTime OccurredAt
);