using MassTransit;
using OrderService.Domain.Orders.Events;
using SharedKernel.Abstractions;
using SharedKernel.IntegrationEvents.Orders;

public class PublishOrderPlacedHandler : IDomainEventHandler<OrderCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishOrderPlacedHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken ct)
    {
        var integrationItems = domainEvent.Items
            .Select(i => new OrderPlacedItemDto(i.ProductId, i.Quantity))
            .ToList();

        var integrationEvent = new OrderPlacedIntegrationEvent(
            Guid.NewGuid(),
            domainEvent.OrderId,
            domainEvent.CustomerId,
            integrationItems,
            DateTime.UtcNow
        );

        // 2. Publish to RabbitMQ
        await _publishEndpoint.Publish(integrationEvent,context =>
        {
            context.MessageId = domainEvent.Id;
        }, ct);
    }
}