using CancellationService.Domain.Cancellations.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents.Cancellations;

namespace CancellationService.Application.Cancellations.Events;

public class CancellationApprovedDomainEventHandler : IDomainEventHandler<CancellationApprovedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CancellationApprovedDomainEventHandler> _logger;

    public CancellationApprovedDomainEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<CancellationApprovedDomainEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(CancellationApprovedDomainEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Cancellation {Id} Approved. Publishing Integration Event...", notification.CancellationId);

        var integrationEvent = new CancellationApprovedIntegrationEvent(
            CancellationId: notification.CancellationId,
            OrderId: notification.OrderId,
            RefundAmount: notification.RefundAmount,
            Remarks: notification.Remarks,
            ApprovedBy: notification.ApprovedBy,
            OccurredOn: notification.OccurredOn
        );

        await _publishEndpoint.Publish(integrationEvent, context =>
        {
            context.MessageId = notification.Id;
        }, ct);
        
        _logger.LogInformation("Integration Event published for Cancellation {Id}", notification.CancellationId);
    }
}