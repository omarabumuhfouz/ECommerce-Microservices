using MassTransit;
using SharedKernel.IntegrationEvents.Customers;
namespace CustomerService.Application.Features.Customers.Commands.EditCustomer;

public class ChangeCustomerNameDomainEventHandler 
    : IDomainEventHandler<ChangeCustomerNameDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ChangeCustomerNameDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(ChangeCustomerNameDomainEvent notification, CancellationToken ct)
    {
        await _publishEndpoint.Publish(new CustomerNameChangedIntegrationEvent
        (
            notification.CustomerId,
            notification.NewName,
            DateTime.UtcNow
        ),context =>
        {
            context.MessageId = notification.Id;
        }, ct);
    }
}