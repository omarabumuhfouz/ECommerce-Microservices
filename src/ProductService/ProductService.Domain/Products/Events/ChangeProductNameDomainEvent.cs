using SharedKernel.Primitives;

public sealed record ChangeProductNameDomainEvent(
Guid ProductId,
string NewName
) : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}