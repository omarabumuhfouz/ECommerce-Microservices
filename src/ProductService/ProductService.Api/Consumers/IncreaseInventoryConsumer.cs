using MassTransit;
using MediatR;
using ProductService.Application.Enums;
using ProductService.Application.Features.Products.Commands.EditStock;
using ProductService.Application.Services;
using SharedKernel.IntegrationEvents.Orders;
using SharedKernel.Primitives.Result;
using SharedKernel.Shared; // Ensure Result<T> is available

namespace ProductService.Api.Consumers;

public class IncreaseInventoryConsumer : IConsumer<OrderCancelledIntegrationEvent>,
                                         IConsumer<OrderItemRemovedIntegrationEvent>,
                                         IConsumer<OrderRefundedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<IncreaseInventoryConsumer> _logger;

    public IncreaseInventoryConsumer(
        ISender sender,
        IIdempotencyService idempotencyService,
        ILogger<IncreaseInventoryConsumer> logger)
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
            tasks.Add(_sender.Send(new EditStockCommand(
                item.ProductId,
                item.Quantity,
                StockOperation.Increase 
            ), context.CancellationToken));
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


            var result = await _sender.Send(new EditStockCommand(
                context.Message.ProductId,
                context.Message.QuantityRemoved,
                StockOperation.Increase 
            ), context.CancellationToken);



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
            tasks.Add(_sender.Send(new EditStockCommand(
                item.ProductId,
                item.Quantity,
                StockOperation.Increase 
            ), context.CancellationToken));
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