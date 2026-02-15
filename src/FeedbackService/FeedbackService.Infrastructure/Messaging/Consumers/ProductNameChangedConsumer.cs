using FeedbackService.Application.Feedbacks.Commands.SyncProductName;
using FeedbackService.Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents.Products;

namespace FeedbackService.Infrastructure.Messaging.Consumers;
public class ProductNameChangedConsumer : IConsumer<ProductNameChangedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILogger<ProductNameChangedConsumer> _logger;
    private readonly IIdempotencyService _idempotencyService;

    public ProductNameChangedConsumer(ISender sender, ILogger<ProductNameChangedConsumer> logger, IIdempotencyService idempotencyService)
    {
        _sender = sender;
        _logger = logger;
        _idempotencyService = idempotencyService;
    }

    public async Task Consume(ConsumeContext<ProductNameChangedIntegrationEvent> context)
    {
        _logger.LogInformation("Received ProductNameChangedIntegrationEvent for ProductId: {ProductId}, NewName: {NewName}",
                    context.Message.ProductId, context.Message.NewName);

        var message = context.Message;

        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(ProductNameChangedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Event {EventName} for Product {ProductId} with MessageId {MessageId} already processed. Skipping.",
                eventName, message.ProductId, messageId);

            return;
        }

        var result = await _sender.Send(new SyncProductNameCommand(message.ProductId, message.NewName));

        if (result.IsFailure)
        {
            _logger.LogError("Failed to sync product name for ProductId: {ProductId}. Error: {Error}",
                message.ProductId, result.TopError.Message);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);

            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Successfully synced product name for ProductId: {ProductId}", message.ProductId);

    }
}