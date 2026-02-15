namespace CancellationService.Application.Cancellations.Commands.UpdateCancellationReason;

public class UpdateCancellationReasonCommandValidator : AbstractValidator<UpdateCancellationReasonCommand>
{
    public UpdateCancellationReasonCommandValidator()
    {
        RuleFor(x => x.CancellationId)
            .ValidateCancellationId();

        RuleFor(x => x.Reason)
            .ValidateReason();
    }
}
