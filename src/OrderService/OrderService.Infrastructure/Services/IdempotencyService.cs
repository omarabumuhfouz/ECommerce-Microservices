using Microsoft.EntityFrameworkCore;
using OrderService.Application.Services;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Outbox;

namespace OrderService.Infrastructure.Services;
public class IdempotencyService : IIdempotencyService
{
    private readonly OrderDbContext _context;

    public IdempotencyService(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid messageId)
    {
        return await _context.InboxMessages
            .AnyAsync(m => m.Id == messageId);
    }

    public async Task MarkAsProcessedAsync(Guid messageId, string name)
    {
        // Check again to be safe (concurrency)
        if (!await ExistsAsync(messageId))
        {
            _context.InboxMessages.Add(new InboxMessage
            {
                Id = messageId,
                Name = name,
                OccurredOn = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }
    }

public async Task MarkAsFailedAsync(Guid messageId, string name, string errorReason)
{
    if (!await ExistsAsync(messageId))
    {
        _context.InboxMessages.Add(new InboxMessage
        {
            Id = messageId,
            Name = name,
            OccurredOn = DateTime.UtcNow,
            Error = errorReason // <--- Save the error here
        });
        await _context.SaveChangesAsync();
    }
}
}