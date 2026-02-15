using MassTransit;
using MediatR;
using ProductService.Application.Enums;
using ProductService.Application.Features.Products.Commands.EditStock;
using ProductService.Application.Services;
using SharedKernel.IntegrationEvents.Orders;
using SharedKernel.Shared;

namespace ProductService.Api.Consumers;

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

        Result<Unit>? stockResult = null;
        if (msg.QuantityDelta > 0)
        {
            // The user ADDED items (e.g., went from 2 to 5, Delta is +3)
            // We need to DECREASE (reserve) 3 more from stock.
            stockResult = await _sender.Send(new EditStockCommand(
               msg.ProductId,
               msg.QuantityDelta,
               StockOperation.Decrease
           ), context.CancellationToken);
        }
        else if (msg.QuantityDelta < 0)
        {
            // The user REMOVED items (e.g., went from 5 to 3, Delta is -2)
            // We need to INCREASE (release) 2 back to stock.
            // Use Math.Abs to turn -2 into 2.
            stockResult = await _sender.Send(new EditStockCommand(
                   msg.ProductId,
                   Math.Abs(msg.QuantityDelta),
                   StockOperation.Increase
               ), context.CancellationToken);
        }

        if (stockResult is null) return;

        if ( stockResult.IsFailure)
        {
            _logger.LogError("Failed to Adust stock for Order {OrderId}. Error: {Error}", 
                context.Message.OrderId, 
                stockResult.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, stockResult.TopError.Message);

            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);
        
        _logger.LogInformation("Stock successfully decreased for Order {OrderId}.", context.Message.OrderId);


    }
}