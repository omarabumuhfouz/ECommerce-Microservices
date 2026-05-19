using CustomerService.Application.Features.Customers.Commands.AddCustomer;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.IntegrationEvents.Customers;
using SharedKernel.IntegrationEvents.Users;

namespace CustomerService.Features.Customers.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly ISender _sender;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UserRegisteredConsumer> _logger;

    public UserRegisteredConsumer(
        ISender sender,
        IPublishEndpoint publishEndpoint,
        ILogger<UserRegisteredConsumer> logger)
    {
        _sender = sender;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Processing registration for User {UserId}...", msg.UserId);

        try
        {
            var command = new AddCustomerCommand(
                msg.UserId,
                msg.FirstName,
                msg.LastName,
                msg.PhoneNumber
            );

            var result = await _sender.Send(command, context.CancellationToken);

            if (result.IsFailure)
            {
                if (result.TopError.Code == "Customer.AlreadyExists") // Replace with your actual error code
                {
                    _logger.LogInformation("Customer {UserId} already exists. Ignoring duplicate message.", msg.UserId);
                    return; 
                }

                _logger.LogWarning("Failed to create customer for User {UserId}. Reason: {Reason}", 
                    msg.UserId, result.TopError.Message);

                await _publishEndpoint.Publish(new CustomerCreationFailedEvent(
                    msg.UserId,
                    Reason: result.TopError.Message, 
                    OccurredAt: DateTime.UtcNow
                ), context.CancellationToken);

                return; 
            }

            _logger.LogInformation("Customer profile created successfully for {UserId}.", msg.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CRITICAL: System Exception while creating customer for {UserId}.", msg.UserId);

            await _publishEndpoint.Publish(new CustomerCreationFailedEvent(
                msg.UserId,
                Reason: $"System Exception: {ex.Message}",
                OccurredAt: DateTime.UtcNow
            ), context.CancellationToken);
        }
    }
}