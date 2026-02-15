namespace SharedKernel.IntegrationEvents.Orders;

public record OrderProcessingIntegrationEvent(
    Guid OrderId,
    Guid CustomerId,
    List<OrderPlacedItemDto> Items,
    DateTime OccurredOn
);
