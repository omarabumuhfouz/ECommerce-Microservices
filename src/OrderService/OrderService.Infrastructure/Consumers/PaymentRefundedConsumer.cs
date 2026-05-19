using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Orders.Commands.UpdateOrderStatusToRefunded;
using OrderService.Application.Services;
using SharedKernel.IntegrationEvents.Payments;

namespace OrderService.Infrastructure.Consumers;

public class PaymentRefundedConsumer : IConsumer<PaymentRefundedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILogger<PaymentRefundedConsumer> _logger;
    private readonly IIdempotencyService _idempotencyService;

    public PaymentRefundedConsumer(ISender sender, ILogger<PaymentRefundedConsumer> logger, IIdempotencyService idempotencyService)
    {
        _sender = sender;
        _logger = logger;
        _idempotencyService = idempotencyService;
    }

    public async Task Consume(ConsumeContext<PaymentRefundedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Payment Refuned for Order {OrderId}. Updating status to Refunded...", message.OrderId);

        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(PaymentRefundedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.",
                eventName, message.OrderId, messageId);
            return;
        }

        var command = new UpdateOrderStatusToRefundedCommand(message.OrderId);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to update order status To Refunded for Order {OrderId}: {Error}", message.OrderId, result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;

            //Future plan to emit evnet but now no i don't want to complicated system more than this
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Change Status to Refunded Successfully for Order :  {OrderId}.", message.OrderId);

    }
}