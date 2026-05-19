using Contracts.Order;
using FeedbackService.Domain.Errors;
using SharedKernel.Primitives.Results;

public class OrderGrpcService : IOrderService
{
    private readonly OrderProtoService.OrderProtoServiceClient _orderClinet;

    public OrderGrpcService(OrderProtoService.OrderProtoServiceClient orderClinet)
    {
        _orderClinet = orderClinet;
    }

    public async Task<Result<bool>> IsProductEligibleForFeedbackAsync(Guid customerId, Guid productId)
    {
        var request = new IsProductEligibleForFeedbackRequest
        {
            CustomerId = customerId.ToString(),
            ProductId = productId.ToString()
        };

        var response = await _orderClinet.IsProductEligibleForFeedbackAsync(request);

        if (response.IsEligible) return Result.Success<bool>(true);
        else return Result.Failure<bool>(DomainErrors.Feedback.ProductNotPurchased);
    }
}