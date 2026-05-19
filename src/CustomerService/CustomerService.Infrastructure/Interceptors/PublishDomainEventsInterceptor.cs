using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel.Primitives.Results;

namespace CustomerService.Infrastructure.Interceptors;

public sealed class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public PublishDomainEventsInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await PublishDomainEvents(eventData.Context, cancellationToken);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext dbContext, CancellationToken ct)
    {
        var aggregateRoots = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .Where(x => x.GetDomainEvents().Any())
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(x => x.GetDomainEvents())
            .ToList();

        // Clear events so they aren't processed again if SaveChanges is called twice
        aggregateRoots.ForEach(x => x.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            // This triggers the MediatR Handler (The Bridge)
            await _publisher.Publish(domainEvent, ct);
        }
    }
}