using InventoryService.Application.Features.Inventory.Commands.ReleaseStock;
using InventoryService.Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents.Orders;
using SharedKernel.Primitives.Results;

namespace InventoryService.Infrastructure.Consumers;

public class ReleaseStockConsumer : IConsumer<OrderCancelledIntegrationEvent>,
                                         IConsumer<OrderItemRemovedIntegrationEvent>,
                                         IConsumer<OrderRefundedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<ReleaseStockConsumer> _logger;

    public ReleaseStockConsumer(
        ISender sender,
        IIdempotencyService idempotencyService,
        ILogger<ReleaseStockConsumer> logger)
    {
        _sender = sender;
        _idempotencyService = idempotencyService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCancelledIntegrationEvent> context)
    {
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderCancelledIntegrationEvent);
        var orderId = context.Message.OrderId;

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.", 
                eventName, orderId, messageId);
            return;
        }

        var tasks = new List<Task<Result<Unit>>>();

        foreach (var item in context.Message.Items)
        {
            tasks.Add(_sender.Send(new ReleaseStockCommand(item.ProductId, orderId), context.CancellationToken));
        }

        var results = await Task.WhenAll(tasks);

        var failure = results.FirstOrDefault(r => r.IsFailure);

        if (failure != null)
        {
            _logger.LogError("Failed to Release stock for Order {OrderId}. Error: {Error}",
                orderId,
                failure.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, failure.TopError.Message);
        
            return; 
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Stock successfully restored (increased) for Order {OrderId}.", orderId);
    }

    public async Task Consume(ConsumeContext<OrderItemRemovedIntegrationEvent> context)
    {
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderItemRemovedIntegrationEvent);
        var orderId = context.Message.OrderId;

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.", 
                eventName, orderId, messageId);
            return;
        }


        var result = await _sender.Send(new ReleaseStockCommand(context.Message.ProductId, orderId), context.CancellationToken);



        if (result.IsFailure)
        {
            _logger.LogError("Failed to restore stock for Order {OrderId}. Error: {Error}",
                orderId,
                result.TopError.Message);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);
        
            return; // Future Plan (emit Event To represnt failer situation)
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Stock successfully restored (increased) for Order {OrderId}.", orderId);

    }

    public async Task Consume(ConsumeContext<OrderRefundedIntegrationEvent> context)
    {
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderRefundedIntegrationEvent);
        var orderId = context.Message.OrderId;

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.", 
                eventName, orderId, messageId);
            return;
        }

        var tasks = new List<Task<Result<Unit>>>();

        foreach (var item in context.Message.Items)
        {
            tasks.Add(_sender.Send(new ReleaseStockCommand(item.ProductId, orderId), context.CancellationToken));
        }

        var results = await Task.WhenAll(tasks);

        var failure = results.FirstOrDefault(r => r.IsFailure);

        if (failure != null)
        {
            _logger.LogError("Failed to restore stock for Order {OrderId}. Error: {Error}",
                orderId,
                failure.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, failure.TopError.Message);
        
            return; 
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Stock successfully restored (increased) for Order {OrderId}.", orderId);

    }
}