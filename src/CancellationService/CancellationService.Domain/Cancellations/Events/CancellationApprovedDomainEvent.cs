using SharedKernel.Primitives; 

namespace CancellationService.Domain.Cancellations.Events;

public record CancellationApprovedDomainEvent(
    Guid CancellationId,
    Guid OrderId,
    decimal RefundAmount, 
    string Remarks,
    Guid ApprovedBy
) : IDomainEvent
{
    public Guid Id { get ; init ; } = Guid.NewGuid();
    public DateTime OccurredOn { get ; init ; } = DateTime.UtcNow;
}