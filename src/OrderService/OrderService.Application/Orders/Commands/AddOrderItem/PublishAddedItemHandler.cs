using MassTransit;
using OrderService.Domain.Orders.Events;
using SharedKernel.Abstractions;
using SharedKernel.IntegrationEvents.Orders;

namespace OrderService.Application.Orders.Commands.AddOrderItem;

public class PublishAddedItemHandler : IDomainEventHandler<ItemAddedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishAddedItemHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(ItemAddedDomainEvent notification, CancellationToken ct)
    {
        var integratedEvent = new OrderItemAddedIntegrationEvent(
                 notification.OrderId,
                 notification.ProductId,
                 notification.Quantity);

        await _publishEndpoint.Publish(integratedEvent, context =>
        {
            context.MessageId = notification.Id;
        }, ct);
        
    }
}