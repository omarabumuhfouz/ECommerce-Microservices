namespace SharedKernel.IntegrationEvents.Products;

public record ProductNameChangedIntegrationEvent(
    Guid ProductId,
    string NewName,
    DateTime OccurredOn);