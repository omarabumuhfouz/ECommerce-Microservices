using OrderService.Application.Services;
using OrderService.Infrastructure.BackgroundJobs;
using Quartz;

namespace OrderService.Infrastructure.Services;

internal class OrderExpirationScheduler : IOrderExpirationScheduler
{
    private readonly ISchedulerFactory _schedulerFactory;

    public OrderExpirationScheduler(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task ScheduleExpirationAsync(Guid orderId, CancellationToken ct)
    {
        var scheduler = await _schedulerFactory.GetScheduler(ct);

        var job = JobBuilder.Create<OrderExpirationJob>()
            .WithIdentity($"Expire-{orderId}", "Orders")
            .UsingJobData("OrderId", orderId)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"Expire-Trigger-{orderId}", "Orders")
            .StartAt(DateTimeOffset.UtcNow.AddMinutes(30))
            .Build();

        await scheduler.ScheduleJob(job, trigger, ct);
    }
}