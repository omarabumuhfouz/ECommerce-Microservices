
using MassTransit;
using Microsoft.Data.SqlClient;
using SharedKernel.Abstractions;
using SharedKernel.IntegrationEvents.Orders;
using OrderService.Domain.Orders.Events;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToShipped;


public class PublishOrderShippedHandler : IDomainEventHandler<OrderShippedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishOrderShippedHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderShippedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new OrderShippedIntegrationEvent(
        notification.OrderId,
        notification.CustomerId,
        notification.Items.Select(i => new OrderPlacedItemDto(i.ProductId, i.Quantity)).ToList(),
        DateTime.UtcNow
        );

        await _publishEndpoint.Publish(integrationEvent, context =>
        {
            context.MessageId = notification.Id;
        }, ct);
    }
}