using AuthService.Application.Features.Users.Commands.DeleteUser;
using MassTransit;
using MassTransit.Mediator;
using MediatR;
using SharedKernel.IntegrationEvents.Customers;

namespace AuthService.Features.Users.Consumers;

public class CustomerCreationFailedConsumer : IConsumer<CustomerCreationFailedEvent>
{
    private readonly ISender _sender;
    private readonly ILogger<CustomerCreationFailedConsumer> _logger;

    public CustomerCreationFailedConsumer(
        ISender sender,
        ILogger<CustomerCreationFailedConsumer> logger
    )
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CustomerCreationFailedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogWarning("Compensating Transaction Started: Customer creation failed for User {UserId}. Reason: {Reason}", 
            message.UserId, message.Reason);

        var command = new DeleteUserCommand(message.UserId);

        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            _logger.LogInformation("Compensating Transaction Successful: User {UserId} has been deleted.", message.UserId);
        }
        else
        {
            _logger.LogError("Compensating Transaction FAILED for User {UserId}. Error: {Error}", 
                message.UserId, result.TopError?.Message);
        }
    }
}