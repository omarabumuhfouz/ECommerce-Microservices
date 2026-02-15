using MassTransit;
using MediatR;
using SharedKernel.IntegrationEvents.Orders;
using SharedKernel.Shared;
using ShoppingCartService.Application.Carts.Commands.CheckoutCart;
using ShoppingCartService.Application.Services;

namespace CartService.Consumers;

public class MarkCartAsCheckedOutConsumer : IConsumer<OrderPlacedIntegrationEvent>
{
    private readonly IIdempotencyService _idempotencyService;
    private readonly ISender _sender;
    private readonly ILogger<MarkCartAsCheckedOutConsumer> _logger;

    public MarkCartAsCheckedOutConsumer(
        IIdempotencyService idempotencyService,
        ISender sender,
        ILogger<MarkCartAsCheckedOutConsumer> logger
    )
    {
        _idempotencyService = idempotencyService;
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderPlacedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Starting Mark Cart as Checkout for Customer: {@CustomerId}", message.CustomerId);

        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OrderPlacedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.",
                eventName, message.OrderId, messageId);
            return;
        }


        var result = await _sender.Send(new CheckoutCartCommand(message.EventId, context.Message.CustomerId));

        if (result.IsFailure)
        {
            _logger.LogError("Failed To Checout Cart for Customer : {@CustomerId}, with Error : {@Error}", message.CustomerId, result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;


            // Publish Event CheckedOut Failed
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Mark Cart as Checkout for Customer :{@CustomerId}, done Successfully.", message.CustomerId); 
    }
}