using Microsoft.Extensions.Logging;
using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;
using SharedKernel.Primitives.Result;

namespace PaymentService.Application.Payments.Commands.MarkAsPending;

public sealed class MarkAsPendingCommandHandler : ICommandHandler<MarkAsPendingCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MarkAsPendingCommandHandler> _logger;

    public MarkAsPendingCommandHandler(IUnitOfWork unitOfWork, ILogger<MarkAsPendingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(MarkAsPendingCommand request, CancellationToken cancellationToken)
    {
       return await _unitOfWork.GetRepository<Payment>()
            .GetSingleBySpecAsync(new GetPaymentByIdSpec(request.PaymentId, true), cancellationToken)
            .ToResult(DomainErrors.Payment.NotFound(request.PaymentId))
            .Bind(payment => payment.MarkAsPending())
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(cancellationToken))
            .Tap(_ => _logger.LogInformation("Successfully marked payment as pending with Id : ${@paymentId}", request.PaymentId))
            .Map(_ => Unit.Value);
    }
}
