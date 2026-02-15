namespace CancellationService.Infrastructure.Outbox;

public sealed class InboxMessage
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
    public string? Error { get; set; } 
}
