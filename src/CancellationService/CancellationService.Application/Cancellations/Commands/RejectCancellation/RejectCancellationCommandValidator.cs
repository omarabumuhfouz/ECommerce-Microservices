using CancellationService.Domain.Constants;

namespace CancellationService.Application.Cancellations.Commands.RejectCancellation;

public class RejectCancellationCommandValidator : AbstractValidator<RejectCancellationCommand>
{
    public RejectCancellationCommandValidator()
    {
        RuleFor(x => x.CancellationId)
            .ValidateCancellationId();

        RuleFor(x => x.ProcessedBy)
            .ValidateAdminId();

        RuleFor(x => x.Remarks)
            .ValidateRemarks();

    }
}