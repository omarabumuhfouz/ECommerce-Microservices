namespace SharedKernel.IntegrationEvents;

public record ProductCreatedIntegrationEvent(
    Guid ProductId,
    int InitialStock,
    int LowStockThreshold,
    DateTime OccurredOn
);