using Microsoft.Extensions.Logging;
using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;
using SharedKernel.Primitives.Result;

namespace PaymentService.Application.Payments.Commands.MarkAsCompleted;

public sealed class MarkAsCompletedCommandHandler : ICommandHandler<MarkAsCompletedCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MarkAsCompletedCommandHandler> _logger;

    public MarkAsCompletedCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<MarkAsCompletedCommandHandler> logger
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(MarkAsCompletedCommand request, CancellationToken ct)
    {
        return  await _unitOfWork.GetRepository<Payment>()
                .GetSingleBySpecAsync(new GetPaymentByIdSpec(request.PaymentId, true), ct)
                .ToResult(DomainErrors.Payment.NotFound(request.PaymentId))
                .Bind(payment => payment.MarkAsCompleted(request.TransactionId))
                .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
                .Tap(_ => _logger.LogInformation("Successfully completed payment with Id : ${@paymentId}", request.PaymentId))
                .Map(_ => Unit.Value);
    }
}
