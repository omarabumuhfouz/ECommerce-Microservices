namespace PaymentService.Application.Payments.Commands.MarkRefundAsComplete;

public sealed class MarkRefundAsCompleteCommandValidator : AbstractValidator<MarkRefundAsCompleteCommand>
{
    public MarkRefundAsCompleteCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.RefundId).NotEmpty();
        RuleFor(x => x.TransactionId).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ApprovedBy).NotEmpty();
    }
}
