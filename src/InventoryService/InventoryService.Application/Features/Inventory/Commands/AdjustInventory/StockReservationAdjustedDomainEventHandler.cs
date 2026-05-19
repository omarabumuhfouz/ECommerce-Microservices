using InventoryService.Domain.InventoryItems.Events;
using MassTransit;
using SharedKernel.IntegrationEvents.Inventory;

namespace InventoryService.Application.Features.Inventory.Commands.AdjustInventory;

public sealed class StockReservationAdjustedDomainEventHandler 
    : IDomainEventHandler<StockReservationAdjustedDomainEvent>
{
    private readonly ILogger<StockReservationAdjustedDomainEventHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReservationAdjustedDomainEventHandler(
        ILogger<StockReservationAdjustedDomainEventHandler> logger,
        IPublishEndpoint publishEndpoint
    )
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(StockReservationAdjustedDomainEvent notification, CancellationToken ct)
    {
        string adjustmentType = notification.QuantityDelta > 0 ? "Increased" : "Decreased";
        
        _logger.LogInformation(
            "Stock Reservation {AdjustmentType} by {Delta} for Order {OrderId}. Product: {ProductId}", 
            adjustmentType,
            Math.Abs(notification.QuantityDelta), 
            notification.OrderId,
            notification.ProductId);

        await _publishEndpoint.Publish(new InventoryAdjustedIntegrationEvent(
        notification.InventoryItemId,
        notification.ProductId,
        notification.OrderId,
        notification.QuantityDelta,
        DateTime.UtcNow
        ), ct);

    }
}