namespace SharedKernel.IntegrationEvents.Customers;
public record CustomerNameChangedIntegrationEvent(
    Guid CustomerId,
    string CustomerName,
    DateTime OccurredOn);