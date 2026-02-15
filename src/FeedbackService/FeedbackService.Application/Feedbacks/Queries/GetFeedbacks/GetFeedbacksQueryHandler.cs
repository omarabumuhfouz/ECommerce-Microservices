namespace FeedbackService.Application.Feedbacks.Queries.GetFeedbacks;

public class GetFeedbacksQueryHandler : IQueryHandler<GetFeedbacksQuery, List<FeedbackDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFeedbacksQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<List<FeedbackDto>>> Handle(GetFeedbacksQuery request, CancellationToken ct)
    {
        return await _unitOfWork.GetRepository<Feedback>()
        .GetListAsync(new GetFeedbacksSpec());
    }
}