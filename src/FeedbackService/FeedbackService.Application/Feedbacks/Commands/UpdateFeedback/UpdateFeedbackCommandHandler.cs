using SharedKernel.Abstractions.Data;
using SharedKernel.Common;

namespace FeedbackService.Application.Feedbacks.Commands.UpdateFeedback;

public class UpdateFeedbackCommandHandler : ICommandHandler<UpdateFeedbackCommand, FeedbackDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateFeedbackCommandHandler> _logger;
    private readonly IMapper _mapper;

    private readonly IRepository<Feedback> _feedbackRepo;

    public UpdateFeedbackCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateFeedbackCommandHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _feedbackRepo = _unitOfWork.GetRepository<Feedback>();
        _logger = logger;
        _mapper = mapper;
    }

    async Task<Result<FeedbackDto>> IRequestHandler<UpdateFeedbackCommand, Result<FeedbackDto>>.Handle(UpdateFeedbackCommand request, CancellationToken ct)
    {

        return await _feedbackRepo.FirstOrDefaultAsync(
            new GetFeedbackByIdAndCustomerSpec(request.FeedbackId, request.CustomerId), ct)
            .ToResult(DomainErrors.Feedback.NotFound(request.FeedbackId, request.CustomerId))
            .Tap(feedback => feedback.Update(request.Rating, request.Comment))
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
            .Tap(_ => _logger.LogInformation("Updated feedback with id {FeedbackId} for customer {CustomerId}", request.FeedbackId, request.CustomerId))
            .Map(feedback => _mapper.Map<FeedbackDto>(feedback));

    }
}