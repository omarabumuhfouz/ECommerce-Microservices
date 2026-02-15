
using MassTransit;
using PaymentService.Domain.Payments.Events;
using SharedKernel.IntegrationEvents.Payments;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;


public class PublishOnlinePaymentCompletedHandler : IDomainEventHandler<OnlinePaymentCompletedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishOnlinePaymentCompletedHandler(IPublishEndpoint publishEndpoint)
        => _publishEndpoint = publishEndpoint;

    public async Task Handle(OnlinePaymentCompletedDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new OnlinePaymentCompletedIntegrationEvent(
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