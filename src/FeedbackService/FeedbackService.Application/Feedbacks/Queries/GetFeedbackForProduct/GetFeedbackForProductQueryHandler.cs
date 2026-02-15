using FeedbackService.Application.Services;

namespace FeedbackService.Application.Feedbacks.Queries.GetFeedbackForProduct;

public class GetFeedbackForProductQueryHandler : IQueryHandler<GetFeedbackForProductQuery, ProductFeedbackDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFeedbackForProductQueryHandler> _logger; 

    public GetFeedbackForProductQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetFeedbackForProductQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ProductFeedbackDto>> Handle(GetFeedbackForProductQuery request, CancellationToken ct)
    {
        var feedbacks = await _unitOfWork.GetRepository<Feedback>()
        .GetListAsync(new GetFeedbacksForProductSpec(request.ProductId));

        if (!feedbacks.Any()) return DomainErrors.Feedback.NotFoundForProduct(request.ProductId);

var productInfo = feedbacks.First(); // Get ProductName from denormalized data
        var averageRating = feedbacks.Average(f => f.Rating.Value);

        var dto = new ProductFeedbackDto(
                productInfo.ProductId,
                productInfo.ProductName,
                Math.Round(averageRating, 1),
                feedbacks.Count,
                feedbacks.Select(f => new CustomerFeedbackDto(
                    f.Id,
                    f.CustomerId,
                    f.CustomerName,
                    f.Rating.Value,
                    f.Comment?.Value,
                    f.CreatedAt,
                    f.UpdatedAt
                )).ToList()
            );

        return dto;
    }
    
}