using InventoryService.Application.Features.Inventory.Commands.Create;
using InventoryService.Application.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents;

namespace InventoryService.Application.Consumers;

public class ProductCreatedConsumer : IConsumer<ProductCreatedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<ProductCreatedConsumer> _logger;

    public ProductCreatedConsumer(
        ISender sender,
        IIdempotencyService idempotencyService,
        ILogger<ProductCreatedConsumer> logger
    )
    {
        _sender = sender;
        _idempotencyService = idempotencyService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var messageId = context.MessageId.GetValueOrDefault();
        var message = context.Message;
        _logger.LogInformation("Received ProductCreatedEvent for Product {ProductId}", message.ProductId);

        if(await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogInformation("Message {MessageId} already processed. Skipping.", context.MessageId.GetValueOrDefault());
            return;
        }

        var command = new CreateInventoryCommand(message.ProductId, message.InitialStock, message.LowStockThreshold);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Failed to create Inventory record for Product {ProductId}. Error: {Error}", message.ProductId, result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, nameof(ProductCreatedIntegrationEvent), result.TopError.Message);

            return;

            // Note Future plan to emit  event represent failer
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, nameof(ProductCreatedIntegrationEvent));



        _logger.LogInformation("Successfully created Inventory Record for Product {ProductId}", message.ProductId);
    }
}