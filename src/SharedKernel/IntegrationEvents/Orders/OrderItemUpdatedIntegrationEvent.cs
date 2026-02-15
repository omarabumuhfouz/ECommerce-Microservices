namespace SharedKernel.IntegrationEvents.Orders;
public record OrderItemUpdatedIntegrationEvent(
    Guid OrderId,
    Guid ProductId,
    int QuantityDelta
);