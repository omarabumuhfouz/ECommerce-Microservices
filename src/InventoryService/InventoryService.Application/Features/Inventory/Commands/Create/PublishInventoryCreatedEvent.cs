using InventoryService.Domain.InventoryItems.Events;
using MassTransit;
using SharedKernel.IntegrationEvents.Inventory;

namespace InventoryService.Application.Features.Inventory.Commands.Create;

public class PublishInventoryCreatedEvent : IDomainEventHandler<InventoryCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishInventoryCreatedEvent(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(InventoryCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _publishEndpoint.Publish(new InventoryCreatedIntegrationEvent(notification.InventoryId, notification.ProductId, notification.InitialStock), cancellationToken);
    }
}