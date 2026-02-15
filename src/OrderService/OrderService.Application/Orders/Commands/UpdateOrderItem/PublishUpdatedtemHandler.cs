using MassTransit;
using OrderService.Domain.Orders.Events;
using SharedKernel.Abstractions;
using SharedKernel.IntegrationEvents.Orders;

namespace OrderService.Application.Orders.Commands.UpdateOrderItem;

public class PublishUpdatedtemHandler : IDomainEventHandler<ItemUpdatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishUpdatedtemHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(ItemUpdatedDomainEvent notification, CancellationToken ct)
    {
        var integratedEvent = new OrderItemUpdatedIntegrationEvent(
           notification.OrderId,
           notification.ProductId,
           notification.QuantityDelta
        );

        await _publishEndpoint.Publish(integratedEvent,context =>
        {
            context.MessageId = notification.Id;
        }, ct);
    
    }
}