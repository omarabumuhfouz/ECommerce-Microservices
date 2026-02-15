using CancellationService.Infrastructure.Data;
using CancellationService.Infrastructure.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using SharedKernel.Primitives;

namespace CancellationService.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob : IJob
{
    private readonly CancellationDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(CancellationDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m =>m.OccurredOnUtc)
            .Take(20)// Batch Size
            .ToListAsync(context.CancellationToken);

        if (!messages.Any()) return;

        var counter = 1;

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

                System.Console.WriteLine("\n\n\n\n________________________________________________________________________________________\n\n\n\n\n\n");
                System.Console.WriteLine($"Messages Counter :  {counter}");
                System.Console.WriteLine("\n\n\n\n________________________________________________________________________________________\n\n\n\n\n\n");

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
