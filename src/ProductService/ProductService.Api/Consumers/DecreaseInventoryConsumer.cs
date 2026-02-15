using MassTransit;
using MediatR;
using ProductService.Application.Enums;
using ProductService.Application.Features.Products.Commands.EditStock;
using ProductService.Application.Services;
using SharedKernel.IntegrationEvents.Orders;
using SharedKernel.Primitives.Result;
using SharedKernel.Shared; // Assuming this is where Result lives

namespace ProductService.Api.Consumers;

public class DecreaseInventoryConsumer : IConsumer<OrderPlacedIntegrationEvent>,
                                         IConsumer<OrderItemAddedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<DecreaseInventoryConsumer> _logger;

    public DecreaseInventoryConsumer(
        ISender sender, 
        IIdempotencyService idempotencyService, 
        ILogger<DecreaseInventoryConsumer> logger)
    {
        _sender = sender;
        _idempotencyService = idempotencyService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderPlacedIntegrationEvent> context)
    {
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderPlacedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Message {MessageId} already processed. Skipping.", messageId);
            return;
        }

        var tasks = new List<Task<Result<Unit>>>(); 

        foreach (var item in context.Message.Items)
        {
            tasks.Add(_sender.Send(new EditStockCommand(
                item.ProductId, 
                item.Quantity, 
                StockOperation.Decrease
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

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Message {MessageId} already processed. Skipping.", messageId);
            return;
        }

            var result = await _sender.Send(new EditStockCommand(
                context.Message.ProductId, 
                context.Message.Quantity, 
                StockOperation.Decrease
            ), context.CancellationToken);


        if (result.IsFailure)
        {
            _logger.LogError("Failed to decrease stock for Order {OrderId}. Error: {Error}", 
                context.Message.OrderId, 
                result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);
        
        _logger.LogInformation("Stock successfully decreased for Order {OrderId}.", context.Message.OrderId);

        
    }
}