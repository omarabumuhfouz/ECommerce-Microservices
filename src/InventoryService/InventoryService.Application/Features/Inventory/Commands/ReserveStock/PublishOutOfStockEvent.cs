using InventoryService.Domain.InventoryItems.Events;
using MassTransit;
using SharedKernel.IntegrationEvents.Inventory;

namespace InventoryService.Application.Features.Inventory.Commands.ReserveStock;

public class PublishOutOfStockEvent : IDomainEventHandler<OutOfStockDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishOutOfStockEvent(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OutOfStockDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new OutOfStockIntegrationEvent(notification.InventoryId, notification.ProductId), cancellationToken);
    }
}