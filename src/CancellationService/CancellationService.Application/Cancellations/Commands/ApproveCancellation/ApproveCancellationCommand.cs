namespace CancellationService.Application.Cancellations.Commands.ApproveCancellation;

public record ApproveCancellationCommand(
    Guid CancellationId,
    string Remakrs,
    decimal charges,
    Guid ProcessBy
) : ICommand<Unit>;