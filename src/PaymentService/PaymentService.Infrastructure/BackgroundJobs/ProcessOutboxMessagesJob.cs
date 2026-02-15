using MediatR;
using Newtonsoft.Json;
using PaymentService.Infrastructure.Data;
using PaymentService.Infrastructure.Outbox;
using Quartz;
using SharedKernel.Primitives;

namespace PaymentService.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private const int BatchSize = 20;
    private readonly PaymentDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(PaymentDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(BatchSize)
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
            }

        }

        await _dbContext.SaveChangesAsync();
    }
}
