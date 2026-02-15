namespace SharedKernel.IntegrationEvents.Orders;

public record OrderDeliveredIntegrationEvent(
    Guid OrderId,
    Guid CustomerId,
    List<OrderPlacedItemDto> Items,
    DateTime OccurredOn
);
