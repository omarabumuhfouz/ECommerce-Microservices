using SharedKernel.Abstractions.Data;
using SharedKernel.Common;

namespace FeedbackService.Application.Feedbacks.Commands.DeleteFeedback;

public class DeleteFeedbackCommandHandler : ICommandHandler<DeleteFeedbackCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Feedback> _feedbackRepo;
    private readonly ILogger<DeleteFeedbackCommandHandler> _logger;

    public DeleteFeedbackCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteFeedbackCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _feedbackRepo = _unitOfWork.GetRepository<Feedback>();
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteFeedbackCommand request, CancellationToken ct)
    {
        _logger.LogInformation(
            "Attempting to delete feedback {FeedbackId} for Customer {CustomerId}...", request.FeedbackId, request.CustomerId);

        return await _feedbackRepo.FirstOrDefaultAsync(
            new GetFeedbackByIdAndCustomerSpec(request.FeedbackId, request.CustomerId), ct)
            .ToResult(DomainErrors.Feedback.NotFound(request.FeedbackId, request.CustomerId))
            .Tap(feedback => _feedbackRepo.Remove(feedback))
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
            .Tap(_ => _logger.LogInformation(
                        "Successfully deleted feedback {FeedbackId} for Customer {CustomerId}.", request.FeedbackId, request.CustomerId))
            .Map(_ => Unit.Value);
    }
}