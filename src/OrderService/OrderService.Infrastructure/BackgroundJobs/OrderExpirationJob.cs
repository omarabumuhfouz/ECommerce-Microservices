using MediatR;
using OrderService.Application.Orders.Commands.ExpireOrder;
using Quartz;

namespace OrderService.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal class OrderExpirationJob : IJob
{
    private readonly ISender _sender; // MediatR

    public OrderExpirationJob(ISender sender) => _sender = sender;

    public async Task Execute(IJobExecutionContext context)
    {
        if (context.MergedJobDataMap.TryGetGuid("OrderId", out var orderId))
        {
            await _sender.Send(new ExpireOrderCommand(orderId));
        }
    }
}