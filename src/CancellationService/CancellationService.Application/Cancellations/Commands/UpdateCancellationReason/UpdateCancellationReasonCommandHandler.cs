using CancellationService.Application.Cancellations.Specifications;
using Microsoft.Extensions.Logging;
using SharedKernel.Common;

namespace CancellationService.Application.Cancellations.Commands.UpdateCancellationReason;

public class UpdateCancellationReasonCommandHandler : ICommandHandler<UpdateCancellationReasonCommand, Unit>
{
    private readonly IRepository<Cancellation> _repository; 
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCancellationReasonCommandHandler> _logger;

    public UpdateCancellationReasonCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateCancellationReasonCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.GetRepository<Cancellation>();
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(UpdateCancellationReasonCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Updating reason for Cancellation {Id}", request.CancellationId);

        return await _repository.FirstOrDefaultAsync(
                new GetCancellationByIdSpec(request.CancellationId, true), ct)

            .ToResult(DomainErrors.Cancellation.NotFound(request.CancellationId))

            .Bind(cancellation => cancellation.UpdateReason(request.Reason))

            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

            .Tap(_ => _logger.LogInformation("Reason for cancellation {Id} updated successfully", request.CancellationId))

            .TapError(error => _logger.LogWarning(
                "Failed to update reason for cancellation {Id}: {Error}", request.CancellationId, error));
    }
}
