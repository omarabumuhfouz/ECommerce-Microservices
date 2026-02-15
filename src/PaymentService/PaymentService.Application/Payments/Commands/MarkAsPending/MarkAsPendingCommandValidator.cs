namespace PaymentService.Application.Payments.Commands.MarkAsPending;

public sealed class MarkAsPendingCommandValidator : AbstractValidator<MarkAsPendingCommand>
{
    public MarkAsPendingCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
    }
}
