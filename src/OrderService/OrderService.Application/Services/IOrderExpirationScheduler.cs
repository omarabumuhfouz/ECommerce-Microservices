namespace OrderService.Application.Services;

public interface IOrderExpirationScheduler
{
    Task ScheduleExpirationAsync(Guid orderId, CancellationToken ct = default);
}