using OrderService.Domain.Orders;
using OrderService.Domain.Orders.Enums;
using SharedKernel.Abstractions;

namespace OrderService.Application.Orders.Queries.IsProductEligibleForFeedback;

public class IsProductEligibleForFeedbackQueryHandler : IQueryHandler<IsProductEligibleForFeedbackQuery, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IsProductEligibleForFeedbackQueryHandler> _logger;

    public IsProductEligibleForFeedbackQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<IsProductEligibleForFeedbackQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<bool>> IRequestHandler<IsProductEligibleForFeedbackQuery, Result<bool>>.Handle(IsProductEligibleForFeedbackQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking feedback eligibility for customer {CustomerId} and product {ProductId}", request.CustomerId, request.ProductId);

        var isEligible = await _unitOfWork.GetRepository<Order>().AnyAsync(
            o => o.CustomerId == request.CustomerId && 
            o.OrderStatus == OrderStatus.Delivered &&
            o.OrderItems.Any(oi => oi.ProductId == request.ProductId)

        ,cancellationToken);
        
        _logger.LogInformation("Feedback eligibility for customer {CustomerId} and product {ProductId} is {IsEligible}", request.CustomerId, request.ProductId, isEligible);

        return isEligible;
    }
}
