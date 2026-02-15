using MassTransit;
using PaymentService.Domain.Payments.Events;
using SharedKernel.IntegrationEvents.Payments;

namespace PaymentService.Application.Payments.Commands.CompleteCodPayment;


public class PublishCodPaymentCompletedHandler : IDomainEventHandler<CodPaymentCompletedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishCodPaymentCompletedHandler(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task Handle(CodPaymentCompletedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new CodPaymentCompletedIntegrationEvent(
             notification.PaymentId,
             notification.OrderId,
            notification.Amount,
            notification.OccurredOn
        );

        await _publishEndpoint.Publish(integrationEvent, context =>
                {
                    context.MessageId = notification.Id;
                }, ct);
    }
}