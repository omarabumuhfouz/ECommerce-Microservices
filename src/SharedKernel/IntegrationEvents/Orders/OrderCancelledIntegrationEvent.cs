namespace SharedKernel.IntegrationEvents.Orders;
public record OrderCancelledIntegrationEvent(
    Guid OrderId,
    Guid CustomerId,
    List<OrderPlacedItemDto> Items,
    DateTime OccurredOn
);
