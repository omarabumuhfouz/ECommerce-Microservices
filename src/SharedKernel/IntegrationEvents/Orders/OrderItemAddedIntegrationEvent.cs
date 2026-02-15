namespace SharedKernel.IntegrationEvents.Orders;
public record OrderItemAddedIntegrationEvent(
    Guid OrderId,
    Guid ProductId,
    int Quantity
) ;