
using MassTransit;
using OrderService.Domain.Orders.Events;
using SharedKernel.IntegrationEvents.Orders;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToRefunded;


public class PublishOrderRefundedIntegrationEventHandler : IDomainEventHandler<OrderRefundedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishOrderRefundedIntegrationEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderRefundedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new OrderRefundedIntegrationEvent(
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
