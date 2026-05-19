using InventoryService.Domain.InventoryItems.Events;
using MassTransit;
using SharedKernel.IntegrationEvents.Inventory;

namespace InventoryService.Application.Features.Inventory.Commands.ReserveStock;

public class PublishStockReservedEvent : IDomainEventHandler<StockReservedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishStockReservedEvent(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(StockReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new StockReservedIntegrationEvent(notification.InventoryId, notification.ProductId, notification.OrderId, notification.Quantity), cancellationToken);
    }
}