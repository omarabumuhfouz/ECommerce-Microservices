using InventoryService.Domain.InventoryItems.Events;
using MassTransit;
using SharedKernel.IntegrationEvents.Inventory;

namespace InventoryService.Application.Features.Inventory.Commands.ReceiveStock;

public class PublishStockReceivedEvent : IDomainEventHandler<StockReceivedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishStockReceivedEvent(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(StockReceivedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new StockReceivedIntegrationEvent(notification.InventoryId, notification.ProductId, notification.Amount), cancellationToken);
    }
}