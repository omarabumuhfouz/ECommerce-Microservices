using SharedKernel.Primitives; 

namespace CancellationService.Domain.Cancellations.Events;

public record CancellationRejectedDomainEvent(
    Guid CancellationId,
    Guid OrderId,
    string Remarks
) : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}