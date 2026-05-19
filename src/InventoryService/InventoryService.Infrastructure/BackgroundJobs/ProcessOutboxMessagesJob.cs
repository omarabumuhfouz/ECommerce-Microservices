using InventoryService.Infrastructure.Data;
using InventoryService.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using SharedKernel.Primitives;

namespace InventoryService.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly InventoryDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly int _batchSize = 20;


    public ProcessOutboxMessagesJob(InventoryDbContext dbContext, IPublisher publisher)
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
            .Take(_batchSize)
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
