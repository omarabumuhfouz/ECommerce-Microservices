using Microsoft.Extensions.Logging;
using CancellationService.Domain.Cancellations.Events;
using SharedKernel.IntegrationEvents.Cancellations;
using MassTransit;

namespace CancellationService.Application.Cancellations.Events;

public class CancellationRejectedDomainEventHandler : IDomainEventHandler<CancellationRejectedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CancellationRejectedDomainEventHandler> _logger;

    public CancellationRejectedDomainEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<CancellationRejectedDomainEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(CancellationRejectedDomainEvent notification, CancellationToken ct)
    {
        _logger.LogInformation(
            "Handling Domain Event: Cancellation {CancellationId} for Order {OrderId} was rejected. Reason: {Reason}", 
            notification.CancellationId, 
            notification.OrderId, 
            notification.Remarks);

        var integrationEvent = new CancellationRejectedIntegrationEvent(
            CancellationId: notification.CancellationId,
            OrderId: notification.OrderId,
            Remarks: notification.Remarks,
            OccurredOn: notification.OccurredOn
        );

        await _publishEndpoint.Publish(integrationEvent,context =>
        {
            context.MessageId = notification.Id;
        } ,ct);

        _logger.LogInformation(
            "Published Integration Event: 'CancellationRejectedIntegrationEvent' for Order {OrderId}. Notification sent to Event Bus.", 
            notification.OrderId);
    }
}