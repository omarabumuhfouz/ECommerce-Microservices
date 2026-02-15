using PaymentService.Application.Services;
using PaymentService.Infrastructure.Data;
using PaymentService.Infrastructure.Outbox;

namespace PaymentService.Infrastructure.Services;

public class IdempotencyService : IIdempotencyService
{
    private readonly PaymentDbContext _context;

    public IdempotencyService(PaymentDbContext context)
    {
        _context = context;
    }

    // NEW: Atomic Check-and-Insert
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