namespace CancellationService.Application.Cancellations.Commands.RejectCancellation;

public record RejectCancellationCommand(
    Guid CancellationId,
    string Remarks,
    Guid ProcessedBy
) : ICommand<Unit>;