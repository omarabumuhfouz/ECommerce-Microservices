using MassTransit;
using MediatR;
using PaymentService.Application.Payments.Commands.ProcessRefund;
using PaymentService.Application.Services;
using SharedKernel.IntegrationEvents.Cancellations;

namespace PaymentService.Api.Consumers;

public class InitiateRefundOnCancellationApprovedConsumer
    : IConsumer<CancellationApprovedIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<InitiateRefundOnCancellationApprovedConsumer> _logger;

    public InitiateRefundOnCancellationApprovedConsumer(
        ISender sender,
        IIdempotencyService idempotencyService,
        ILogger<InitiateRefundOnCancellationApprovedConsumer> logger
    )
    {
        _sender = sender;
        _idempotencyService = idempotencyService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CancellationApprovedIntegrationEvent> context)
    {
        var message = context.Message;
        var messageId = context.MessageId.GetValueOrDefault();
        var eventName = nameof(CancellationApprovedIntegrationEvent);

        if (await _idempotencyService.ExistsAsync(messageId))
        {
            _logger.LogWarning("Message {MessageId} is already being processed or finished. Skipping duplicate.", messageId);
            return;
        }

        _logger.LogInformation("Starting refund for Order {OrderId} due to Cancellation {Id}",
            message.OrderId, message.CancellationId);

        // 2. Perform the Business Logic
        var result = await _sender.Send(new ProcessRefundCommand(
            message.OrderId,
            message.CancellationId,
            message.ApprovedBy,
            message.RefundAmount,
            message.Remarks
        ));

        // 3. UPDATE the existing row based on the outcome
        if (result.IsFailure)
        {
            _logger.LogError("Refund failed for Order {Id}: {Error}",
                message.OrderId, result.TopError);

            await _idempotencyService.MarkAsFailedAsync(messageId, eventName, result.TopError.Message);
            return;
        }

        await _idempotencyService.MarkAsProcessedAsync(messageId, eventName);

        _logger.LogInformation("Payment Status changed to Refunded Successfully for Order: {OrderId}.", message.OrderId);
    }
}