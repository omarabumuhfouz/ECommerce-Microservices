namespace ProductService.Infrastructure.InboxMessages;

public class InboxMessage
{
    public Guid Id { get; set; } 
    public string Name { get; set; } 
    public DateTime OccurredOn { get; set; } 
    public string? Error { get; set; } 
}
