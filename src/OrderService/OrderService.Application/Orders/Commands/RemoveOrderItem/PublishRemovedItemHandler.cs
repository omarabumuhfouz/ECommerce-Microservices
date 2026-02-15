using MassTransit;
using OrderService.Domain.Orders.Events;
using SharedKernel.Abstractions;
using SharedKernel.IntegrationEvents.Orders;

namespace OrderService.Application.Orders.Commands.RemoveOrderItem;

public class PublishRemovedItemHandler : IDomainEventHandler<ItemRemovedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishRemovedItemHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(ItemRemovedDomainEvent notification, CancellationToken ct)
    {
        var integrationatedEvent = new OrderItemRemovedIntegrationEvent(
            notification.OrderId,
            notification.ProductId,
            notification.QuantityRemoved
        );

        await _publishEndpoint.Publish(integrationatedEvent,context =>
        {
            context.MessageId = notification.Id;
        } , ct);
    }
}