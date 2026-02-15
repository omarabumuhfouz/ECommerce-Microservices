namespace CancellationService.Application.Cancellations.Commands.ApproveCancellation;

public class ApproveCancellationCommandValidator : AbstractValidator<ApproveCancellationCommand>
{
    public ApproveCancellationCommandValidator()
    {
        RuleFor(x => x.CancellationId)
            .ValidateCancellationId();

        RuleFor(x => x.ProcessBy)
            .ValidateAdminId();
    }
}