namespace PaymentService.Infrastructure.Outbox;

public sealed class InboxMessage
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; } 
}
