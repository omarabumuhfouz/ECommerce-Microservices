using MassTransit;
using MediatR;
using SharedKernel.IntegrationEvents.Orders;
using ShoppingCartService.Application.Carts.Commands.RestoreCart;
using ShoppingCartService.Application.Services;

namespace CartService.Consumers;

public class RestoreCartConsumer 
    : IConsumer<OrderCancelledIntegrationEvent>
{

    private readonly ISender _sender;
    private readonly ILogger<RestoreCartConsumer> _logger;
    private readonly IIdempotencyService _idempotencyService;

    public RestoreCartConsumer(
        ISender sender,
        ILogger<RestoreCartConsumer> logger,
        IIdempotencyService idempotencyService
    )
    {
        _sender = sender;
        _logger = logger;
        _idempotencyService = idempotencyService;
    }

    public async Task Consume(ConsumeContext<OrderCancelledIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Starting Restoring cart for Customer {CustomerId} due to Order {OrderId} cancellation.",
                    message.CustomerId, message.OrderId);

        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderCancelledIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.",
                eventName, message.OrderId, messageId);
            return;
        }

        var result = await _sender.Send(new RestoreCartCommand(message.CustomerId));

        if (result.IsFailure)
        {
            _logger.LogError("Failed to restore cart for Customer {CustomerId}: {Error}",
                message.CustomerId, result.TopError.Message);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;

            // My Note: Future plan publish Restoration Failed
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Restor Cart for Customer :{@CustomerId}, done Successfully.", message.CustomerId); 

    }
}