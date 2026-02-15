using Microsoft.Extensions.Logging;
using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;
using SharedKernel.Primitives.Result;

namespace PaymentService.Application.Payments.Commands.MarkAsRefunded;

public sealed class MarkAsRefundedCommandHandler : ICommandHandler<MarkAsRefundedCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MarkAsRefundedCommandHandler> _logger;

    public MarkAsRefundedCommandHandler(IUnitOfWork unitOfWork, ILogger<MarkAsRefundedCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(MarkAsRefundedCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetRepository<Payment>()
            .GetSingleBySpecAsync(new GetPaymentByOrderIdSpec(request.OrderId, true), cancellationToken)
            .ToResult(DomainErrors.Payment.NotFoundByOrder(request.OrderId))
            .Bind(payment => payment.MarkAsRefunded())
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(cancellationToken))
            .Tap(_ => _logger.LogInformation("Successfully marked payment as refunded with OrderId : ${@OrderId}", request.OrderId))
            .Map(_ => Unit.Value);
    }
}
