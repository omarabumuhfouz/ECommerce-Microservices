using MassTransit;
using OrderService.Application.Services;
using SharedKernel.IntegrationEvents.Payments;

namespace OrderService.Api.Consumers;

public class PaymentFailedConsumer : IConsumer<PaymentFailedIntegrationEvent>
{
    private readonly ILogger<PaymentFailedConsumer> _logger;
    private readonly IIdempotencyService _idempotencyService;

    public PaymentFailedConsumer(ILogger<PaymentFailedConsumer> logger, IIdempotencyService idempotencyService)
    {
        _logger = logger;
        _idempotencyService = idempotencyService;
    }

    public async Task Consume(ConsumeContext<PaymentFailedIntegrationEvent> context)
    {
        var message = context.Message;
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(PaymentFailedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.",
                eventName, message.OrderId, messageId);
            return;
        }


        // Background Job Will handle this situation after 30 minutes
        _logger.LogWarning("Payment failed for Order {Id}. User has until 30-min window expires.", context.Message.OrderId);
    }
}