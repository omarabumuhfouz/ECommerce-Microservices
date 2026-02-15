namespace PaymentService.Application.Payments.Commands.MarkAsFailed;

public sealed class MarkAsFailedCommandValidator : AbstractValidator<MarkAsFailedCommand>
{
    public MarkAsFailedCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
    }
}
