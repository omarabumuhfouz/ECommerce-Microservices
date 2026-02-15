public interface IOrderService
{
    Task<Result<bool>> IsProductEligibleForFeedbackAsync(Guid customerId, Guid productId);
}