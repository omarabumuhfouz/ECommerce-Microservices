
using MassTransit;
using PaymentService.Domain.Payments.Events;
using SharedKernel.IntegrationEvents.Payments;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;


public class PublishPaymentRefundedHandler : IDomainEventHandler<PaymentRefundedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishPaymentRefundedHandler(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task Handle(PaymentRefundedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new PaymentRefundedIntegrationEvent(
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