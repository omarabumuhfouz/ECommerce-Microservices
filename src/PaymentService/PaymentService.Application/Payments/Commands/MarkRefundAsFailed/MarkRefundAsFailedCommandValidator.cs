namespace PaymentService.Application.Payments.Commands.MarkRefundAsFailed;

public sealed class MarkRefundAsFailedCommandValidator : AbstractValidator<MarkRefundAsFailedCommand>
{
    public MarkRefundAsFailedCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.RefundId).NotEmpty();
    }
}
