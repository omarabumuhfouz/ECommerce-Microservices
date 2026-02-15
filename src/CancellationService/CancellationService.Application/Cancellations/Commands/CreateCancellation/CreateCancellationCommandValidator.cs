using System.Data;

namespace CancellationService.Application.Cancellations.Commands.CreateCancellation;

public class CreateCancellationCommandValidator : AbstractValidator<CreateCancellationCommand>
{
    public CreateCancellationCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Validation.OrderIdRequired)
            .WithMessage("Order ID is required.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Reason.Empty)
            .WithMessage("Cancellation reason is required.")
            .MaximumLength(500)
            .WithErrorCode(ErrorCodes.Reason.TooLong)
            .WithMessage("Reason cannot exceed 500 characters.");
    }
}