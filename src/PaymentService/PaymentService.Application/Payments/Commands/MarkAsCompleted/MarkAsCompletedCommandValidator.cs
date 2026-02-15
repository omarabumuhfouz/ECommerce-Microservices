namespace PaymentService.Application.Payments.Commands.MarkAsCompleted;

public sealed class MarkAsCompletedCommandValidator : AbstractValidator<MarkAsCompletedCommand>
{
    public MarkAsCompletedCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
    }
}
