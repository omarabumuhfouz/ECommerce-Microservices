namespace SharedKernel.IntegrationEvents.Orders;

public record OrderRefundedIntegrationEvent(
    Guid OrderId,
    Guid CustomerId,
    List<OrderPlacedItemDto> Items,
    DateTime OccurredOn
);
