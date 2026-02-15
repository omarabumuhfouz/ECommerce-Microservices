namespace SharedKernel.IntegrationEvents.Orders;

public record OrderShippedIntegrationEvent(
    Guid OrderId,
    Guid CustomerId,
    List<OrderPlacedItemDto> Items,
    DateTime OccurredOn
);
