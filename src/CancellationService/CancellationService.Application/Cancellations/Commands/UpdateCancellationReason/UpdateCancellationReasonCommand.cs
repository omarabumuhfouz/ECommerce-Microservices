namespace CancellationService.Application.Cancellations.Commands.UpdateCancellationReason;

public record UpdateCancellationReasonCommand(
    Guid CancellationId,
    string Reason
) : ICommand<Unit>;
