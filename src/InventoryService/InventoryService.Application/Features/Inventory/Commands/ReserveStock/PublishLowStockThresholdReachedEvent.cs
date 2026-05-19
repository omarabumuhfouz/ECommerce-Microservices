using InventoryService.Domain.InventoryItems.Events;
using MassTransit;
using SharedKernel.IntegrationEvents.Inventory;

namespace InventoryService.Application.Features.Inventory.Commands.ReserveStock;

public class PublishLowStockThresholdReachedEvent : IDomainEventHandler<LowStockThresholdReachedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishLowStockThresholdReachedEvent(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(LowStockThresholdReachedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new LowStockThresholdReachedIntegrationEvent(notification.InventoryId, notification.ProductId, notification.AvailableQuantity), cancellationToken);
    }
}