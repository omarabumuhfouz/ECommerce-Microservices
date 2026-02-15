using FeedbackService.Application.Feedbacks.Commands.SyncCustomerName;
using FeedbackService.Application.Services;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents.Customers;

namespace FeedbackService.Infrastructure.Messaging.Consumers;

public class CustomerNameChangedConsumer : IConsumer<CustomerNameChangedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILogger<CustomerNameChangedConsumer> _logger;
    private readonly IIdempotencyService _idempotencyService;

    public CustomerNameChangedConsumer(
        ISender sender,
        ILogger<CustomerNameChangedConsumer> logger,
        IIdempotencyService idempotencyService)
    {
        _sender = sender;
        _logger = logger;
        _idempotencyService = idempotencyService;
    }

    public async Task Consume(ConsumeContext<CustomerNameChangedIntegrationEvent> context)
    {
        _logger.LogInformation("Received CustomerNameChangedIntegrationEvent for CustomerId: {CustomerId}, NewName: {NewName}",
            context.Message.CustomerId, context.Message.CustomerName);

        var message = context.Message;


        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(CustomerNameChangedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Customer {CustomerId} with MessageId {MessageId} already processed. Skipping.",
                eventName, message.CustomerId, messageId);

            return;
        }


        var result = await _sender.Send(new SyncCustomerNameCommand(message.CustomerId, message.CustomerName));

        if (result.IsFailure)
        {
            _logger.LogError("Failed to sync customer name for CustomerId: {CustomerId}. Error: {Error}",
                message.CustomerId, result.TopError.Message);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Successfully synced customer name for CustomerId: {CustomerId}", message.CustomerId);
    }
}