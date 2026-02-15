using RefundService.Domain.Constants;

namespace PaymentService.Application.Payments.Commands.ProcessRefund;

public class ProcessRefundCommandValidator : AbstractValidator<ProcessRefundCommand>
{
    public ProcessRefundCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.CancellationId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Remarks).MaximumLength(RefundConstants.MaxRemarksLength);
    }
}