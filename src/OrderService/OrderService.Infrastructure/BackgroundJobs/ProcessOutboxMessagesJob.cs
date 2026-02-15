using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Outbox;
using Quartz;
using SharedKernel.Primitives;

namespace OrderService.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly OrderDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(OrderDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(om => om.OccurredOnUtc)
            .Take(20)// Batch Size
            .ToListAsync(context.CancellationToken);

        if (!messages.Any()) return;

        foreach (OutboxMessage outboxMessage in messages)
        {
            try
            {
                IDomainEvent? domainEvent = JsonConvert
                                .DeserializeObject<IDomainEvent>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });

                if (domainEvent is null) continue;


                await _publisher.Publish(domainEvent, context.CancellationToken);

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;

            }
            catch (Exception ex)
            {
                outboxMessage.Error = ex.Message;
                // Implemenet a retry mechanism and error handling as needed
            }

        }

        await _dbContext.SaveChangesAsync();
    }
}
