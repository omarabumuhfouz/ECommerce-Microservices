using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;

namespace PaymentService.Application.Payments.Commands.MarkRefundAsFailed;

public sealed class MarkRefundAsFailedCommandHandler : ICommandHandler<MarkRefundAsFailedCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public MarkRefundAsFailedCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(MarkRefundAsFailedCommand request, CancellationToken ct)
    {

        return await _unitOfWork.GetRepository<Payment>()
            .GetSingleBySpecAsync(new GetPaymentByIdSpec(request.PaymentId, true), ct)
            .ToResult(DomainErrors.Payment.NotFound(request.PaymentId))
            .Ensure(payment => !payment.Method.SupportsAutomaticRefund, DomainErrors.Payment.AutomaticRefundNotSupported)
            .Bind(payment => payment.FailRefund(request.RefundId))
            .Tap(_ => _unitOfWork.SaveChangesAsync(ct));

    }
}
