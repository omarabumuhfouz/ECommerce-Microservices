using InventoryService.Application.Features.Inventory.Commands.DispatchStock;
using InventoryService.Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents.Orders;
using SharedKernel.Primitives.Results;

namespace InventoryService.Infrastructure.Consumers;
public class DispatchStockConsumer : IConsumer<OrderShippedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<DispatchStockConsumer> _logger;

    public DispatchStockConsumer(
        ISender sender,
        IIdempotencyService idempotencyService,
        ILogger<DispatchStockConsumer> logger)
    {
        _sender = sender;
        _idempotencyService = idempotencyService;
        _logger = logger;
    }



    public async Task Consume(ConsumeContext<OrderShippedIntegrationEvent> context)
    {
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderShippedIntegrationEvent);
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
            tasks.Add(_sender.Send(new DispatchStockCommand(item.ProductId, orderId), context.CancellationToken));
        }

        var results = await Task.WhenAll(tasks);

        var failure = results.FirstOrDefault(r => r.IsFailure);

        if (failure != null)
        {
            _logger.LogError("Failed to Dispatch stock for Order {OrderId}. Error: {Error}",
                orderId,
                failure.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, failure.TopError.Message);

            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Stock successfully Dispatched (increased) for Order {OrderId}.", orderId);

    }
}