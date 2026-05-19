using InventoryService.Domain.InventoryItems.Events;
using MassTransit;
using SharedKernel.IntegrationEvents.Inventory;

namespace InventoryService.Application.Features.Inventory.Commands.DispatchStock;

public class PublishStockDispatchedEvent : IDomainEventHandler<StockDispatchedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishStockDispatchedEvent(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(StockDispatchedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new StockDispatchedIntegrationEvent(notification.InventoryId, notification.ProductId, notification.OrderId), cancellationToken);
    }
}