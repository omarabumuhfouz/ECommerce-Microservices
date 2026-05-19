using InventoryService.Application.Features.Inventory.Commands.ReserveStock;
using InventoryService.Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents.Orders;
using SharedKernel.Shared;

namespace InventoryService.Infrastructure.Consumers;

public class ReserveStockConsumer : IConsumer<OrderPlacedIntegrationEvent>,
                                         IConsumer<OrderItemAddedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<ReserveStockConsumer> _logger;

    public ReserveStockConsumer(
        ISender sender, 
        IIdempotencyService idempotencyService, 
        ILogger<ReserveStockConsumer> logger)
    {
        _sender = sender;
        _idempotencyService = idempotencyService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderPlacedIntegrationEvent> context)
    {
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderPlacedIntegrationEvent);
        var message = context.Message;
        if(message is null)
        {
            _logger.LogError("");
            return;
        }

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Message {MessageId} already processed. Skipping.", messageId);
            return;
        }

        var tasks = new List<Task<Result<Unit>>>(); 

        foreach (var item in context.Message.Items)
        {
            tasks.Add(_sender.Send(new ReserveStockCommand(
                item.ProductId,
                context.Message.OrderId,
                item.Quantity
            ), context.CancellationToken));
        }

        var results = await Task.WhenAll(tasks);

        var failure = results.FirstOrDefault(r => r.IsFailure);

        if (failure != null)
        {
            _logger.LogError("Failed to decrease stock for Order {OrderId}. Error: {Error}", 
                context.Message.OrderId, 
                failure.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, failure.TopError.Message);

            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);
        
        _logger.LogInformation("Stock successfully decreased for Order {OrderId}.", context.Message.OrderId);
    }

    public async Task Consume(ConsumeContext<OrderItemAddedIntegrationEvent> context)
    {
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderItemAddedIntegrationEvent);
        var message = context.Message;
        if(message is null)
        {
            _logger.LogError("");
            return;
        }

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Message {MessageId} already processed. Skipping.", messageId);
            return;
        }

            var result = await _sender.Send(new ReserveStockCommand(
                message.ProductId, 
                message.OrderId,
                message.Quantity
            ), context.CancellationToken);


        if (result.IsFailure)
        {
            _logger.LogError("Failed to decrease stock for Order {OrderId}. Error: {Error}", 
                message.OrderId, 
                result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);
        
        _logger.LogInformation("Stock successfully decreased for Order {OrderId}.", message.OrderId);

        
    }
}