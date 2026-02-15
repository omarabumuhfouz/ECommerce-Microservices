using Microsoft.Extensions.Logging;
using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;
using SharedKernel.Primitives.Result;

namespace PaymentService.Application.Payments.Commands.MarkAsFailed;

public sealed class MarkAsFailedCommandHandler : ICommandHandler<MarkAsFailedCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MarkAsFailedCommandHandler> _logger;

    public MarkAsFailedCommandHandler(IUnitOfWork unitOfWork, ILogger<MarkAsFailedCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(MarkAsFailedCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetRepository<Payment>()
            .GetSingleBySpecAsync(new GetPaymentByIdSpec(request.PaymentId, true), cancellationToken)
            .ToResult(DomainErrors.Payment.NotFound(request.PaymentId))
            .Bind(payment => payment.MarkAsFailed())
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(cancellationToken))
            .Tap(_ => _logger.LogInformation("Successfully marked payment as failed with Id : ${@paymentId}", request.PaymentId))
            .Map(_ => Unit.Value);
    }
}
