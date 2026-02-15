namespace OrderService.Application.Services;

public interface IIdempotencyService
{
    Task<bool> ExistsAsync(Guid messageId);
    Task MarkAsProcessedAsync(Guid messageId, string name);
    Task MarkAsFailedAsync(Guid messageId, string name, string errorReason);
}