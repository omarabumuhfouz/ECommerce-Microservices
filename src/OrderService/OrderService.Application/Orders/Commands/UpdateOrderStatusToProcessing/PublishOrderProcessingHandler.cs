
using MassTransit;
using Microsoft.Data.SqlClient;
using SharedKernel.Abstractions;
using SharedKernel.IntegrationEvents.Orders;
using OrderService.Domain.Orders.Events;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToProcessing;


public class PublishOrderProcessingHandler : IDomainEventHandler<OrderProcessingDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishOrderProcessingHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderProcessingDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new OrderProcessingIntegrationEvent(
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