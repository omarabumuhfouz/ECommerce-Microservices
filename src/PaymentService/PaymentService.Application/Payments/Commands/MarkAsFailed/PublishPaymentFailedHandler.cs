
using MassTransit;
using PaymentService.Domain.Payments.Events;
using SharedKernel.IntegrationEvents.Payments;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;


public class PublishPaymentFailedHandler : IDomainEventHandler<PaymentFailedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishPaymentFailedHandler(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task Handle(PaymentFailedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new PaymentFailedIntegrationEvent(
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