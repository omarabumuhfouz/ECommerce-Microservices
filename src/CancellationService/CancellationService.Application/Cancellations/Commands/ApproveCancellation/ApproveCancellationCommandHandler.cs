
using CancellationService.Application.Cancellations.Specifications;
using Microsoft.Extensions.Logging;
using SharedKernel.Common;

namespace CancellationService.Application.Cancellations.Commands.ApproveCancellation;

public class ApproveCancellationCommandHandler : ICommandHandler<ApproveCancellationCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ApproveCancellationCommandHandler> _logger;

    public ApproveCancellationCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ApproveCancellationCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ApproveCancellationCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Handling ApproveCancellationCommand for CancellationId: {CancellationId} by UserId: {UserId}", request.CancellationId, request.ProcessBy);

        return  await _unitOfWork.GetRepository<Cancellation>()
        .FirstOrDefaultAsync(new GetCancellationByIdSpec(request.CancellationId, true), ct)
        .ToResult(DomainErrors.Cancellation.NotFound(request.CancellationId))
        .Bind(cancellation => cancellation.Approve(request.Remakrs, request.charges,request.ProcessBy))
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Tap(_ => _logger.LogInformation("Successfully approved cancellation with Id: {CancellationId}", request.CancellationId))
        .Map(_ => Unit.Value);
    }
}