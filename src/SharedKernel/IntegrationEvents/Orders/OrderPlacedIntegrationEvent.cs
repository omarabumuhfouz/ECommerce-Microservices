namespace SharedKernel.IntegrationEvents.Orders;

public record OrderPlacedIntegrationEvent(
    Guid EventId,
    Guid OrderId,
    Guid CustomerId,
    List<OrderPlacedItemDto> Items,
    DateTime OccurredOn
);

public record OrderPlacedItemDto(Guid ProductId, int Quantity);