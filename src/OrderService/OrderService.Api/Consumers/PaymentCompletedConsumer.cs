using MassTransit;
using MediatR;
using SharedKernel.IntegrationEvents.Payments;
using OrderService.Application.Orders.Commands.UpdateOrderStatusToDelivered;
using OrderService.Application.Orders.Commands.UpdateOrderStatusToProcessing;
using OrderService.Application.Services;

namespace OrderService.Api.Consumers;

public class PaymentCompletedConsumer : IConsumer<CodPaymentCompletedIntegrationEvent>,
                                       IConsumer<OnlinePaymentCompletedIntegrationEvent>
{
    private readonly ISender _sender; 
    private readonly ILogger<PaymentCompletedConsumer> _logger;
    private readonly IIdempotencyService _idempotencyService;

    public PaymentCompletedConsumer(
        ISender sender,
        IIdempotencyService idempotencyService,
        ILogger<PaymentCompletedConsumer> logger
        )
    {
        _sender = sender;
        _idempotencyService = idempotencyService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CodPaymentCompletedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Payment completed for Order {OrderId}. Updating status to delivered...", message.OrderId);

        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(CodPaymentCompletedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.",
                eventName, message.OrderId, messageId);
            return;
        }

        var command = new UpdateOrderStatusToDeliveredCommand(message.OrderId, message.PaymentId);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to update order status for Order {OrderId}: {Error}", message.OrderId, result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;
            //Future plan to emit evnet but now no i don't want to complicated system more than this
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Change Status to Delivered Successfully for Order :  {OrderId}.", message.OrderId);
    }

    public async Task Consume(ConsumeContext<OnlinePaymentCompletedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Payment completed for Order {OrderId}. Updating status to processing...", message.OrderId);
        
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(OnlinePaymentCompletedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Order {OrderId} with MessageId {MessageId} already processed. Skipping.",
                eventName, message.OrderId, messageId);
            return;
        }
   

        var command = new UpdateOrderStatusToProcessingCommand(message.OrderId, message.PaymentId);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to update order status for Order {OrderId}: {Error}", message.OrderId, result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;

            //Future plan to emit evnet but now no i don't want to complicated system more than this
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Change Status to Processing Successfully for Order :  {OrderId}.", message.OrderId);

    }
}