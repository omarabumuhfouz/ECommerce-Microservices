using InventoryService.Domain.InventoryItems.Events;
using MassTransit;
using SharedKernel.IntegrationEvents.Inventory;

namespace InventoryService.Application.Features.Inventory.Commands.ReleaseStock;

public class PublishReservationReleasedEvent : IDomainEventHandler<ReservationReleasedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishReservationReleasedEvent(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(ReservationReleasedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new StockReleasedIntegrationEvent(notification.InventoryId, notification.ProductId, notification.OrderId), cancellationToken);
    }
}