
using MassTransit;
using Microsoft.Data.SqlClient;
using SharedKernel.Abstractions;
using SharedKernel.IntegrationEvents.Orders;
using OrderService.Domain.Orders.Events;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;


public class PublishOrderCanceledHandler : IDomainEventHandler<OrderCanceledDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishOrderCanceledHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderCanceledDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new OrderCancelledIntegrationEvent(
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