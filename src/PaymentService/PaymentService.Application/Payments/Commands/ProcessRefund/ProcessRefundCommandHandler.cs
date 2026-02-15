using Microsoft.Extensions.Logging;
using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;

namespace PaymentService.Application.Payments.Commands.ProcessRefund;

public class ProcessRefundCommandHandler : ICommandHandler<ProcessRefundCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentGateway _paymentGateway;
    private readonly ILogger<ProcessRefundCommandHandler> _logger;

    public ProcessRefundCommandHandler(
        IUnitOfWork unitOfWork, 
        IPaymentGateway paymentGateway,
        ILogger<ProcessRefundCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _paymentGateway = paymentGateway;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ProcessRefundCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<Payment>();

        if (await repository.IsExistsAsync(r => r.Refunds.Any(r => r.CancellationId == request.CancellationId)))
        {
            return Unit.Value; // Return Success: Work is already done
        }
        
        var payment = await repository.GetSingleBySpecAsync(
            new GetPaymentByOrderIdSpec(request.OrderId, true), cancellationToken);

        if (payment is null) return DomainErrors.Payment.NotFoundByOrder(request.OrderId);

        var refundResult = payment.RefundPayment(
            request.CancellationId, 
            request.Amount, 
            request.Remarks);

        if (refundResult.IsFailure) return refundResult.TopError;

        var refundEntity = payment.Refunds.Last();

        if (payment.Method.SupportsAutomaticRefund)
        {
            var gatewayResult = await _paymentGateway.RefundAsync(request.Amount, cancellationToken);

            if (gatewayResult.IsSuccess)
            {
                var result = payment.CompleteRefund(
                    refundEntity.Id, 
                    request.ApprovedByUserId,
                    gatewayResult.TransactionId 
                ); // System process ID

                if(result.IsFailure)
                {
                    payment.FailRefund(refundEntity.Id);
                    _logger.LogError("Completing Refund failed for Payment {Id} after successful gateway refund", payment.Id);
                    return result.TopError;
                }
            }
            else
            {
                payment.FailRefund(refundEntity.Id);
                _logger.LogError("Gateway Refund failed for Payment {Id}", payment.Id);
            }
        }
        else 
        {
            // For COD/Manual, we leave the Refund entity in 'Pending' status.
            // We do NOT call payment.MarkAsRefunded() yet.
            _logger.LogInformation("Manual refund {Id} created and awaiting approval.", refundEntity.Id);
        }

var statusResult = payment.MarkAsRefunded();
    if (statusResult.IsFailure) return statusResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}