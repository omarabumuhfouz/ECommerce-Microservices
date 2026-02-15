namespace CancellationService.Application.Cancellations.Commands.CreateCancellation;

public record CreateCancellationCommand(
        Guid OrderId,
        string Reason
) : ICommand<CancellationDto>;
