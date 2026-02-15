
using MassTransit;
using PaymentService.Domain.Payments.Events;
using SharedKernel.IntegrationEvents.Payments;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;


public class PublishPaymentPendinedHandler : IDomainEventHandler<PaymentPendingDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishPaymentPendinedHandler(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task Handle(PaymentPendingDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new PaymentPendingIntegrationEvent(
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