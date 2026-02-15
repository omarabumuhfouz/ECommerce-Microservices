using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;

namespace PaymentService.Application.Payments.Commands.MarkRefundAsComplete;

public sealed class MarkRefundAsCompleteCommandHandler : ICommandHandler<MarkRefundAsCompleteCommand, Unit>
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkRefundAsCompleteCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _paymentRepository = unitOfWork.GetRepository<Payment>();
    }

    public async Task<Result<Unit>> Handle(MarkRefundAsCompleteCommand request, CancellationToken ct)
    {
        return await _paymentRepository.GetSingleBySpecAsync(new GetPaymentByIdSpec(request.PaymentId, true), ct)
        .ToResult(DomainErrors.Payment.NotFound(request.PaymentId))
        .Ensure(payment => !payment.Method.SupportsAutomaticRefund , DomainErrors.Payment.AutomaticRefundNotSupported)
        .Bind(payment => payment.CompleteRefund(request.RefundId, request.ApprovedBy, request.TransactionId))
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct));
    }
}
