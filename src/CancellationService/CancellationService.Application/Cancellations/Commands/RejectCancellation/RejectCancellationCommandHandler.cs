using CancellationService.Application.Cancellations.Specifications;
using Microsoft.Extensions.Logging;
using SharedKernel.Common;

namespace CancellationService.Application.Cancellations.Commands.RejectCancellation;

public class RejectCancellationCommandHandler : ICommandHandler<RejectCancellationCommand, Unit>
{
    private readonly IRepository<Cancellation> _repository; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RejectCancellationCommandHandler> _logger;

    public RejectCancellationCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RejectCancellationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.GetRepository<Cancellation>();
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(RejectCancellationCommand request, CancellationToken ct)
{
    _logger.LogInformation("Processing rejection for Cancellation {Id}", request.CancellationId);

    return await _repository.FirstOrDefaultAsync(
            new GetCancellationByIdSpec(request.CancellationId, true), ct)

        .ToResult(DomainErrors.Cancellation.NotFound(request.CancellationId))

        .Bind(cancellation => cancellation.Reject(request.Remarks, request.ProcessedBy))

        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(_ => _logger.LogInformation("Cancellation {Id} rejected successfully", request.CancellationId))

        .TapError(error => _logger.LogWarning(
            "Failed to reject cancellation {Id}: {Error}", request.CancellationId, error));
}
}