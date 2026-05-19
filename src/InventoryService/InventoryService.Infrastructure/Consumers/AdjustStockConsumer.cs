using InventoryService.Application.Features.Inventory.Commands.AdjustInventory;
using InventoryService.Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents.Orders;

namespace InventoryService.Infrastructure.Consumers;

public class AdjustStockDeltaConsumer : IConsumer<OrderItemUpdatedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<AdjustStockDeltaConsumer> _logger;

    public AdjustStockDeltaConsumer(
        ISender sender,
        IIdempotencyService idempotencyService,
        ILogger<AdjustStockDeltaConsumer> logger
    )
    {
        _sender = sender;
        _idempotencyService = idempotencyService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderItemUpdatedIntegrationEvent> context)
    {
         var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderItemUpdatedIntegrationEvent);
        var msg = context.Message;

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Message {MessageId} already processed. Skipping.", messageId);
            return;
        }

    var command = new AdjustInventoryCommand(
        context.Message.ProductId, 
        context.Message.OrderId, 
        context.Message.QuantityDelta 
    );

    var result = await _sender.Send(command, context.CancellationToken);

        if (result is null) return;

        if ( result.IsFailure)
        {
            _logger.LogError("Failed to Adust stock for Order {OrderId}. Error: {Error}", 
                context.Message.OrderId, 
                result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);
    
        _logger.LogInformation("Stock successfully decreased for Order {OrderId}.", context.Message.OrderId);
    }
}