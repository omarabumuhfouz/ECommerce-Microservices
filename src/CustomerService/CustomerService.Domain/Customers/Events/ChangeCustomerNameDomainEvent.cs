using SharedKernel.Primitives;

public record ChangeCustomerNameDomainEvent(Guid CustomerId, string NewName) : IDomainEvent
{
    public Guid Id { get ; init ; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}