namespace FeedbackService.Application.Feedbacks.Commands.SyncCustomerName;

public class SyncCustomerCommandNameHandler : ICommandHandler<SyncCustomerNameCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    public SyncCustomerCommandNameHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Unit>> Handle(SyncCustomerNameCommand request, CancellationToken ct)
    {
        var feedbacks = await _unitOfWork.GetRepository<Feedback>()
                .GetListAsync(new  GetFeedbacksByCustomerId(request.CustomerId, withTracking: true), ct);

        foreach (var feedback in feedbacks) feedback.SyncCustomerName(request.NewName);

        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
