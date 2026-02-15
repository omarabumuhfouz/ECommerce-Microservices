namespace SharedKernel.IntegrationEvents.Orders;

public record OrderItemRemovedIntegrationEvent(
    Guid OrderId,
    Guid ProductId,
    int QuantityRemoved
);