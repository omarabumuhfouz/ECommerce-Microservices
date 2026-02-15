    namespace FeedbackService.Application.Feedbacks.Commands.SyncProductName;

public class SyncProductNameCommandHandler : ICommandHandler<SyncProductNameCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public SyncProductNameCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<Unit>> Handle(SyncProductNameCommand request, CancellationToken ct)
    {
        var feedbacks = await _unitOfWork.GetRepository<Feedback>()
        .GetListAsync(new GetFeedbacksForProductSpec(request.ProductId, true), ct);

        foreach (var feedback in feedbacks) feedback.SyncProductName(request.NewName);

        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}