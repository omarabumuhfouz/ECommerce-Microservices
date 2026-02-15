using SharedKernel.Abstractions.Data;
using SharedKernel.Common;

namespace FeedbackService.Application.Feedbacks.Commands.SubmitFeedback;

public class SubmitFeedbackCommandHandler : ICommandHandler<SubmitFeedbackCommand, FeedbackDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SubmitFeedbackCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;

    private readonly IRepository<Feedback> _repository;

    public SubmitFeedbackCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<SubmitFeedbackCommandHandler> logger,
        IMapper mapper,
        IOrderService orderService
    )
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.GetRepository<Feedback>();
        _logger = logger;
        _mapper = mapper;
        _orderService = orderService;
    }

    public async Task<Result<FeedbackDto>> Handle(SubmitFeedbackCommand request, CancellationToken ct)
    {
        return await _orderService.IsProductEligibleForFeedbackAsync(request.CustomerId, request.ProductId)
        .Ensure(async _ => await EnsureCustomerHasNotReviewed(request.CustomerId, request.ProductId, ct))
        .Bind(_ => CreateFeedback(request))
        .Tap(async feedback => await _repository.AddAsync(feedback, ct))
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Tap(feedback => _logger.LogInformation("Submitted feedback with id {FeedbackId} for customer {CustomerId}", feedback.Id, request.CustomerId))
        .Map(feedback => _mapper.Map<FeedbackDto>(feedback));
    }

    private async Task<Result> EnsureCustomerHasNotReviewed(
        Guid customerId,
        Guid productId,
        CancellationToken ct)
    {
        var exists = await _repository.AnyAsync(
            f => f.CustomerId == customerId && f.ProductId == productId,
            ct);

        if (exists) return DomainErrors.Feedback.AlreadyReviewed;

        return Result.Success();
    }


    private Result<Feedback> CreateFeedback(SubmitFeedbackCommand request)
    {
        return Feedback.Create(
            request.CustomerId,
            request.ProductId,
            request.CustomerName,
            request.ProductName,
            request.Rating,
            request.Comment);
    }

}